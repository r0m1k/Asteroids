using Asteroids.Services;
using Asteroids.Services.ECS;
using Asteroids.UIEntityData;
using Services;
using StateMachine;

namespace Asteroids.StateMachine
{
    public class CleanUpState : State
    {
        private readonly IServiceProvider _services;
        private readonly IEntityWorldService _entityWorldService;
        private readonly IEntitySystemService _entitySystemService;
        private readonly IDataService _dataService;

        public CleanUpState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
            _entityWorldService = _services.Get<IEntityWorldService>();
            _entitySystemService = _services.Get<IEntitySystemService>();
            _dataService = _services.Get<IDataService>();
        }

        public override void Enter()
        {
            _entitySystemService.Stop();
            _entityWorldService.CleanUp();

            CleanUpData();

            _stateMachine.Enter<SpawnState>();
        }

        public override void Exit()
        {
        }

        private void CleanUpData()
        {
            var score = _dataService.GetFirst<ScoreData>();
            score.Data.Score = 0;
            score.Notify();
        }
    }
}