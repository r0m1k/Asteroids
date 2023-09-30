using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using System.Linq;
using Asteroids.ECS.Parameters;
using Asteroids.Services;

namespace Asteroids.ECS.Systems
{
    public class AsteroidSpawnSystem : EnemySpawnEntitySystem<AsteroidSpawnCooldownComponent>
    {
        private AsteroidSpawnParameters _spawnParameters;

        public AsteroidSpawnSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService) : base(dataService, asteroidsRulesService, randomService) { }

        protected override float CooldownTime => _randomService.GetFloat(_spawnParameters.SpawnCooldown);
        protected override int MaximumSimultaneousEntities => _spawnParameters.MaximumSimultaneousCount;
        protected override float SafeSpawnRadiusAroundPlayer => _spawnParameters.SafeSpawnRadiusAroundPlayer;

        protected override bool GetSpawnParameters()
        {
            _spawnParameters = _dataService.GetScriptable<AsteroidSpawnParameters>();
            return _spawnParameters;
        }

        protected override int CurrentSpawnedEntities()
        {
            return World.FilterByComponents<IsAsteroidComponent>().Excl<IsDebrisComponent>().Count();
        }

        protected override int GenerateSpawnCount()
        {
            return _randomService.GetInt(_spawnParameters.SpawnCount);
        }

        protected override long SpawnEntity()
        {
            return EntityWorldService.SpawnAsteroid();
        }
    }
}