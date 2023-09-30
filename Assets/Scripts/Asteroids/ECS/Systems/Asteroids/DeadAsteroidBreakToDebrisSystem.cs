using Asteroids.ECS.Components;
using ECS;
using System.Linq;
using Asteroids.Services;

namespace Asteroids.ECS.Systems
{
    // ToDo: move from fixed update with temp entity to next update!
    public class DeadAsteroidBreakToDebrisSystem : SpawnEntitySystem, IEntitySystemRequireFixedUpdate
    {
        public DeadAsteroidBreakToDebrisSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService) : base(dataService, asteroidsRulesService, randomService) {}

        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsAsteroidComponent, IsDeadComponent>()
                .Excl<IsTakeDeadlyDamageComponent>()
                .Excl<IsDebrisComponent>()
                .ToArray();
            foreach (var entity in entities) EntityWorldService.SpawnDebris(entity);
        }
    }
}
