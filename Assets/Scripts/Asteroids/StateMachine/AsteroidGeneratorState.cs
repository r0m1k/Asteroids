using Asteroids.Services.ECS;
using Services;
using StateMachine;

namespace Asteroids.StateMachine
{
    public class AsteroidGeneratorState : State
    {
        private readonly IServiceProvider _services;

        public AsteroidGeneratorState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
        }

        public override void Enter()
        {
            var entityFactory = _services.Get<IEntityFactoryService>();
            entityFactory.GenerateAsteroids();

            _stateMachine.Enter<FirstDelayForLoadScreenState>();
        }

        public override void Exit()
        {
        }
    }
}
