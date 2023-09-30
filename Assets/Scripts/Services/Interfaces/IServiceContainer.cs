using System.Collections.Generic;

namespace Services
{
    public interface IServiceContainer : IServiceProvider, IEnumerable<IService>
    {
        void AddConfiguration(ServicesConfiguration configuration);

        IServiceContainer Register<T>(T service) where T : class, IService;

        void InitializeServices();
        void StopServices();
    }
}