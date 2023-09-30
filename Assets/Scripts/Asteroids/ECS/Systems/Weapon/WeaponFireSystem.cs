using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using ECS;
using System.Linq;
using Asteroids.Services;

namespace Asteroids.ECS.Systems
{
    public abstract class WeaponFireSystem : SpawnEntitySystem, IEntitySystemRequireUpdate
    {
        protected WeaponType _weaponType;
        protected WeaponFireSystem(WeaponType weaponType, IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService) : base(dataService, asteroidsRulesService, randomService)
        {
            _weaponType = weaponType;
        }

        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsWeaponWantFire, WeaponComponent>().ToArray();
            foreach (var entity in entities)
            {
                var weapon = entity.GetComponent<WeaponComponent>();
                if (weapon.Parameters.Type != _weaponType) continue;

                // ToDo: move to clean up system!?
                entity.RemoveComponent<IsWeaponWantFire>();

                TryFire(entity, weapon);
            }
        }

        private void TryFire(IEntity entity, WeaponComponent weapon)
        {
            if (!CanFire(weapon)) return;

            UpdateWeaponState(entity, weapon);
            DoFire(entity, weapon);
        }

        private bool CanFire(WeaponComponent weapon)
        {
            var shipEntity = World.Get(weapon.ShipEntityId);
            if (shipEntity == null) return false;

            return weapon.Parameters.IsUnlimitedBullets || weapon.Bullets > 0;
        }

        private void UpdateWeaponState(IEntity entity, WeaponComponent weapon)
        {
            if (!weapon.Parameters.IsUnlimitedBullets) --weapon.Bullets;

            if (weapon.Parameters.HasFireCooldown)
            {
                var fireCooldown = entity.AddComponent<WeaponFireCooldownComponent>();
                fireCooldown.Value = weapon.Parameters.FireCooldown;
            }
        }

        protected abstract void DoFire(IEntity weaponEntity, WeaponComponent weaponComponent);
    }
}