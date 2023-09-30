using ECS;
using Services;

namespace Asteroids.Services.ECS
{
    public interface IEntitySystemService : IService
    {
        IEntitySystemService AddSystem<T>() where T : class, IEntitySystem, new();
        IEntitySystemService AddSystem<T>(T instance) where T : class, IEntitySystem;

        void RemoveSystem<T>() where T : class, IEntitySystem;
        void RemoveSystem<T>(T instance) where T : class, IEntitySystem;

        void Start();
        void Stop();
    }
}
