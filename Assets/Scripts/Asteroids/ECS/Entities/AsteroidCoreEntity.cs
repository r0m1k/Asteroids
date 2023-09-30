using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Entities
{
    public abstract class AsteroidCoreEntity : Entity
    {
        protected override void AddComponents()
        {
            AddComponent<IsAsteroidComponent>();
            AddComponent<AsteroidComponent>();

            AddComponent<IsPhysicComponent>();
            AddComponent<IsViewComponent>();
            AddComponent<TransformComponent>();
            AddComponent<MovementComponent>();
            AddComponent<HealthComponent>();
            AddComponent<DamageComponent>();
            AddComponent<ScoreComponent>();
        }
    }
}