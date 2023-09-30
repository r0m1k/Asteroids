using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class CheckHealthEntitySystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<HealthComponent>();
            foreach (var entity in entities)
            {
                var health = entity.GetComponent<HealthComponent>();
                if (health.Value > 0) continue;

                entity.AddComponentIfNotExists<IsDeadComponent>();
            }
        }
    }
}