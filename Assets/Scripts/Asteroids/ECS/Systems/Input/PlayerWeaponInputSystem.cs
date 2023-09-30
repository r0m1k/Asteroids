using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PlayerWeaponInputSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        private readonly IPlayerShipInput _playerInput;

        public PlayerWeaponInputSystem(IPlayerShipInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsPlayerComponent, WeaponComponent>().Excl<WeaponFireCooldownComponent>();
            foreach (var entity in entities)
            {
                var weapon = entity.GetComponent<WeaponComponent>();

                if (IsWeaponFireByPriority(weapon.Priority)) entity.AddComponentIfNotExists<IsWeaponWantFire>();
            }
        }

        private bool IsWeaponFireByPriority(WeaponPriorityType priority)
        {
            switch (priority)
            {
                case WeaponPriorityType.Primary:
                    return _playerInput.IsFirePrimaryWeapon();
                case WeaponPriorityType.Secondary:
                    return _playerInput.IsFireSecondaryWeapon();
                default:
                    return false;
            }
        }
    }
}