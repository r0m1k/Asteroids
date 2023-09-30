using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Entities
{
    public abstract class BulletEntity : Entity
    {
        protected override void AddComponents()
        {
            AddComponent<IsBulletComponent>();
            AddComponent<BulletComponent>();
            AddComponent<DamageComponent>();

            AddComponent<TransformComponent>();
            AddComponent<IsViewComponent>();
        }
    }
}