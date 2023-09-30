using ECS;
using Services;

namespace Asteroids.Services.ECS
{
    public interface IEntityWorldService : IService, IEntityRemover
    {
        IReadOnlyEntityWorld World { get; }
        IReadOnlyEntityViewWorld ViewWorld { get; }
        IEntityRemover EntityRemover { get; }

        void CreateNewWorld();
        void CleanUp();

        long SpawnPlayer();
        long SpawnAlien();
        long SpawnAsteroid();
        long[] SpawnDebris(IEntity asteroid);

        long SpawnBullet(long weaponId);

        long SpawnEffect(EntityView effectViewPrefab);

        bool RemoveEntity(long id);
    }
}
