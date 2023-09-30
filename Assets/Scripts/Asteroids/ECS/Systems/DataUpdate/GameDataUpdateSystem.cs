using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class GameDataUpdateSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        private readonly IDataService _dataService;

        public GameDataUpdateSystem(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void Update(float deltaTime)
        {
        }
    }

    
}