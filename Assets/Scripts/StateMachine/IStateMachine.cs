namespace StateMachine
{
    public interface IStateMachine
    {
        IState Current { get; }

        void AddState<T>(T state) where T : class, IState;
        void Enter<T>() where T : class, IState;
    }
}