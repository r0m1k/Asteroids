using Asteroids.Services;
using Asteroids.Services.ECS;
using Services;
using StateMachine;

namespace Asteroids.StateMachine
{
    public class SpawnState : State
    {
        private readonly IServiceProvider _services;
        private readonly IEntityWorldService _entityWorldService;
        private readonly IEntitySystemService _entitySystemService;
        private readonly IUIService _uiService;

        public SpawnState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
            _entityWorldService = _services.Get<IEntityWorldService>();
            _entitySystemService = _services.Get<IEntitySystemService>();
            _uiService = _services.Get<IUIService>();
        }

        public override void Enter()
        {
            _entityWorldService.CreateNewWorld();
            _entitySystemService.Start();

            _uiService.ShowHud();

            _stateMachine.Enter<PlayState>();
        }

        public override void Exit()
        {
        }
    }
}