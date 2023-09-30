using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PlayerSpawnSystem : SpawnEntitySystem, IEntitySystemRequireStart
    {
        public PlayerSpawnSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService) : base(dataService, asteroidsRulesService, randomService) { }

        public void WorldStarted()
        {
            var playerId = EntityWorldService.SpawnPlayer();

            AssignAvailableWeapons(playerId);
        }

        private void AssignAvailableWeapons(long playerId)
        {
            var entity = World.Get(playerId);

            var availableWeapons = entity.GetComponent<AvailableWeaponsComponent>();
            if (availableWeapons == null) return;

            if (availableWeapons.Ids.Count > 0) AssignWeaponPriority(availableWeapons.Ids[0], WeaponPriorityType.Primary);
            if (availableWeapons.Ids.Count > 1) AssignWeaponPriority(availableWeapons.Ids[1], WeaponPriorityType.Secondary);
        }

        private void AssignWeaponPriority(long weaponId, WeaponPriorityType priority)
        {
            var entity = World.Get(weaponId);
            var weapon = entity.GetComponent<WeaponComponent>();

            weapon.Priority = priority;
        }
    }
}