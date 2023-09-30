using Asteroids.Services;
using Asteroids.StateMachine;
using Services;
using UnityEngine;

namespace Asteroids
{
    public class Bootstrapper : MonoBehaviour
    {
        public ServicesConfiguration ServicesConfiguration;

        private LifecycleStateMachine _stateMachine;
        private IServiceContainer _services;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _services = new ServiceContainer();
            _services.AddConfiguration(ServicesConfiguration);
            _services.InitializeServices();

            _stateMachine = new LifecycleStateMachine(_services);
            _stateMachine.Run();
        }

        private void OnDestroy()
        {
            _services.StopServices();
        }
    }
}