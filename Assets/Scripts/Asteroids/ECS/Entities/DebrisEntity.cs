using Asteroids.ECS.Components;

namespace Asteroids.ECS.Entities
{
    public class DebrisEntity : AsteroidCoreEntity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<IsDebrisComponent>();
        }
    }
}