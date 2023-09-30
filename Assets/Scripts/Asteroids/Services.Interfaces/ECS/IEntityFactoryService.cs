using ECS;
using Services;

namespace Asteroids.Services.ECS
{
    public interface IEntityFactoryService : IService
    {
        EntityViewPair SpawnEmptyEntity();

        EntityViewPairs SpawnPlayerShip();
        EntityViewPairs SpawnAlienShip();

        EntityViewPairs SpawnBullet(IEntity weapon);

        EntityViewPairs SpawnEffect(IEntityView effectViewPrefab);

        void GenerateAsteroids();
        EntityViewPairs SpawnAsteroid();
        EntityViewPairs SpawnDebris(IEntity asteroid);

        void Destroy(IEntity entity, IEntityView entityView);
    }
}