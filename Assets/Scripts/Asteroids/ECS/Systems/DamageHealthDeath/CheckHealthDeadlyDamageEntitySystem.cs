using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class CheckHealthDeadlyDamageEntitySystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<HealthComponent, IsTakeDeadlyDamageComponent>();
            foreach (var entity in entities)
            {
                entity.AddComponentIfNotExists<IsDeadComponent>();
            }
        }
    }
}