using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using Asteroids.Services;
using Asteroids.UIEntityData;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PlayerWeaponDataUpdateSystem : EntitySystem, IEntitySystemRequireUpdate, IEntitySystemRequireStart
    {
        private readonly WeaponType _weaponType;
        private readonly IDataService _dataService;

        private IDataWriter<PlayerWeaponData> _playerWeapons;

        public PlayerWeaponDataUpdateSystem(WeaponType weaponType, IDataService dataService)
        {
            _dataService = dataService;
            _weaponType = weaponType;
        }

        public void WorldStarted()
        {
            _playerWeapons = _dataService.GetFirstOrCreate<PlayerWeaponData>();
            _playerWeapons.Data.Type = _weaponType;
        }

        public void Update(float deltaTime)
        {
            var entity = FindWeapon(WeaponType.Laser);
            if (entity == null) return;

            var weapon = entity.GetComponent<WeaponComponent>();
            var recharge = entity.GetComponent<WeaponRechargeCooldownComponent>();
            var fire = entity.GetComponent<WeaponFireCooldownComponent>();

            _playerWeapons.Data.Bullets = weapon.Bullets;
            _playerWeapons.Data.MaxBullets = weapon.Parameters.MaxBullets;

            _playerWeapons.Data.RechargeCooldown = recharge?.Value ?? 0;
            _playerWeapons.Data.FireCooldown = fire?.Value ?? 0;

            _playerWeapons.Notify();
        }

        private IEntity FindWeapon(WeaponType weaponType)
        {
            var entities = World.FilterByComponents<IsPlayerComponent, WeaponComponent>();
            foreach (var entity in entities)
            {
                var weapon = entity.GetComponent<WeaponComponent>();
                if (weapon.Parameters.Type == weaponType) return entity;
            }

            return default;
        }
    }
}