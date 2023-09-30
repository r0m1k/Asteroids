using Asteroids.ECS.Components;

namespace Asteroids.ECS.Entities
{
    public class CannonBulletEntity : BulletEntity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<IsCannonBulletComponent>();

            AddComponent<IsPhysicComponent>();
            AddComponent<MovementComponent>();
        }
    }
}