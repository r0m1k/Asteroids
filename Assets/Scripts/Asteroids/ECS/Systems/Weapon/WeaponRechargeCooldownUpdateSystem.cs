using Asteroids.ECS.Components;
using ECS;
using Infrastructure;

namespace Asteroids.ECS.Systems
{
    public class WeaponRechargeCooldownUpdateSystem : EntitySystem, IEntitySystemRequireLateUpdate
    {
        public void LateUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<WeaponComponent, WeaponRechargeCooldownComponent>();
            foreach (var entity in entities)
            {
                var rechargeCooldown = entity.GetComponent<WeaponRechargeCooldownComponent>();
                rechargeCooldown.Value -= deltaTime;
                if (rechargeCooldown.Value > 0) continue;

                entity.RemoveComponent<WeaponRechargeCooldownComponent>();

                var weapon = entity.GetComponent<WeaponComponent>();
                ++weapon.Bullets;
                weapon.Bullets = weapon.Bullets.ClampMax(weapon.Parameters.MaxBullets);
            }
        }
    }
}

namespace Asteroids
{
    public class WeaponRechargeUpdateSystem : EntitySystem, IEntitySystemRequireLateUpdate
    {
        public void LateUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<WeaponComponent>().Excl<WeaponRechargeCooldownComponent>();
            foreach (var entity in entities)
            {
                var weapon = entity.GetComponent<WeaponComponent>();
                if (weapon.Bullets >= weapon.Parameters.MaxBullets) continue;

                var rechargeCooldown = entity.AddComponent<WeaponRechargeCooldownComponent>();
                rechargeCooldown.Value = weapon.Parameters.RechargeCooldown;
            }
        }
    }

}