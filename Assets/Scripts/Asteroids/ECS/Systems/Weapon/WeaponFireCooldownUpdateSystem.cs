using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class WeaponFireCooldownUpdateSystem : EntitySystem, IEntitySystemRequireLateUpdate
    {
        public void LateUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<WeaponComponent, WeaponFireCooldownComponent>();
            foreach (var entity in entities)
            {
                var fireCooldown = entity.GetComponent<WeaponFireCooldownComponent>();
                fireCooldown.Value -= deltaTime;
                if (fireCooldown.Value > 0) continue;

                entity.RemoveComponent<WeaponFireCooldownComponent>();
            }
        }
    }
}
