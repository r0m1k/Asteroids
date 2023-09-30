using Services;

namespace Asteroids.StateMachine
{
    public class LifecycleStateMachine : global::StateMachine.StateMachine
    {
        private readonly IServiceProvider _services;

        public LifecycleStateMachine(IServiceProvider services)
        {
            _services = services;

            AddStates();
        }

        private void AddStates()
        {
            // register services
            AddState(new BootstrapState(this, _services));
            // generate specifications for asteroids
            AddState(new AsteroidGeneratorState(this, _services));
            // generate specifications for asteroids
            AddState(new FirstDelayForLoadScreenState(this, _services));
            // create ui, start ECS
            AddState(new SpawnState(this, _services));
            //
            AddState(new PlayState(this, _services));
            // clean resources -> go to spawn play state?
            AddState(new CleanUpState(this, _services));
        }

        public void Run()
        {
            Enter<BootstrapState>();
        }
    }
}