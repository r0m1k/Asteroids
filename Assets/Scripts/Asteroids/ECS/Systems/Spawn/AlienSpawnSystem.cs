using Asteroids.ECS.Components;
using Asteroids.ECS.Entities;
using Asteroids.ECS.Parameters;
using Asteroids.Services;
using Asteroids.Services.ECS;

namespace Asteroids.ECS.Systems
{
    public class AlienSpawnSystem : EnemySpawnEntitySystem<AlienSpawnCooldownComponent>
    {
        private AlienSpawnParameters _spawnParameters;

        public AlienSpawnSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService) : base(dataService, asteroidsRulesService, randomService) { }

        protected override float CooldownTime => _randomService.GetFloat(_spawnParameters.SpawnCooldown);
        protected override int MaximumSimultaneousEntities => _spawnParameters.MaximumSimultaneousCount;
        protected override float SafeSpawnRadiusAroundPlayer => _spawnParameters.SafeSpawnRadiusAroundPlayer;

        protected override bool GetSpawnParameters()
        {
            _spawnParameters = _dataService.GetScriptable<AlienSpawnParameters>();
            return _spawnParameters;
        }

        protected override int CurrentSpawnedEntities()
        {
            var currentCount = 0;
            World.Visit<AlienShipEntity>(e => ++currentCount);

            return currentCount;
        }

        protected override int GenerateSpawnCount()
        {
            return _randomService.GetInt(_spawnParameters.SpawnCount);
        }

        protected override long SpawnEntity()
        {
            return EntityWorldService.SpawnAlien();
        }
    }
}