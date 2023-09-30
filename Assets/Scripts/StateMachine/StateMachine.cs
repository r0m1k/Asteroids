using System;
using System.Collections.Generic;

namespace StateMachine
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>(5);
        public IState Current { get; protected set; }

        public void AddState<T>(T state) where T : class, IState
        {
            _states.Add(typeof(T), state);
        }

        public void Enter<T>() where T : class, IState
        {
            Current?.Exit();

            Current = GetState<T>();
            Current.Enter();
        }

        private T GetState<T>() where T : class, IState
        {
            if (!_states.TryGetValue(typeof(T), out var state)) throw new ArgumentException();
            return state as T;
        }
    }
}