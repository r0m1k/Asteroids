using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Services;

namespace Asteroids.Services
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>(20);

        public void AddConfiguration(ServicesConfiguration configuration)
        {
            foreach (var serviceConfiguration in configuration.Services)
            {
                if (serviceConfiguration.ValidationState != ServiceTypeValidationStateType.Valid) continue;

                var serviceType = Type.GetType(serviceConfiguration.TypeName);
                var parameters = new object[serviceConfiguration.ConstructorParameters.Count];
                for (var i = 0; i < parameters.Length; ++i)
                {
                    switch (serviceConfiguration.ConstructorParameters[i].Type)
                    {
                        case ServiceTypeConstructorParameterType.Service:
                            var interfaceType = Type.GetType(serviceConfiguration.ConstructorParameters[i].TypeAssemblyQualifiedName);
                            _services.TryGetValue(interfaceType, out var interfaceInstance);
                            parameters[i] = interfaceInstance;
                            break;
                        case ServiceTypeConstructorParameterType.Object:
                            parameters[i] = serviceConfiguration.ConstructorParameters[i].Object;
                            break;
                        default:
                            continue;
                    }
                }
                var serviceInstance = (IService) Activator.CreateInstance(serviceType, parameters);

                foreach (var implementedInterface in serviceConfiguration.ImplementedInterfaces)
                {
                    if (!implementedInterface.Register) continue;

                    var interfaceType = Type.GetType(implementedInterface.InterfaceAssemblyQualifiedName);
                    _services.Add(interfaceType, serviceInstance);
                }
            }
        }

        public T Get<T>() where T : class, IService
        {
            if (!_services.TryGetValue(typeof(T), out var service)) throw new ArgumentException();
            return (T) service;
        }

        public IServiceContainer Register<T>(T service) where T : class, IService
        {
            _services.Add(typeof(T), service);
            return this;
        }

        public void InitializeServices()
        {
            foreach (var service in _services)
            {
                if (service.Value is IRequireInitialize requireInitializeService)
                {
                    requireInitializeService.Initialize();
                }
            }
        }

        public void StopServices()
        {
            foreach (var service in _services)
            {
                if (service.Value is IRequireCleanUp requireCleanUp)
                {
                    requireCleanUp.CleanUp();
                }
            }
        }

        public IServiceContainer Register<T, TInstance>(TInstance service) where T : class, IService where TInstance : T
        {
            _services.Add(typeof(T), service);
            return this;
        }

        #region IEnumerator<T>
        public IEnumerator<IService> GetEnumerator()
        {
            return _services.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}