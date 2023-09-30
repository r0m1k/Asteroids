using Asteroids.ECS.Components;

namespace Asteroids.ECS.Entities
{
    public class LazerBulletEntity : BulletEntity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<IsLazerBulletComponent>();
            AddComponent<IsDeadlyDamageComponent>();
        }
    }
}