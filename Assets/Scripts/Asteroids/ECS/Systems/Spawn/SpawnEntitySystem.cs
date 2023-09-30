using Asteroids.Services;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Systems
{
    public abstract class SpawnEntitySystem : EntitySystem, IEntitySystemRequireWorldService
    {
        public IEntityWorldService EntityWorldService { get; set; }

        protected readonly IReadOnlyDataService _dataService;
        protected readonly IAsteroidsRulesService _asteroidsRulesService;
        protected readonly IRandomService _randomService;


        protected SpawnEntitySystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService)
        {
            _dataService = dataService;
            _asteroidsRulesService = asteroidsRulesService;
            _randomService = randomService;
        }
    }
}