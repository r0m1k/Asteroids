namespace StateMachine
{
    public abstract class State : IState
    {
        protected readonly IStateMachine _stateMachine;

        protected State(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}