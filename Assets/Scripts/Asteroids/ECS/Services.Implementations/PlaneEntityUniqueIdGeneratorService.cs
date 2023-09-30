using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Services
{
    public class PlaneEntityUniqueIdGeneratorService : IEntityUniqueIdGeneratorService
    {
        private long _lastEntityId = Constants.InvalidEntityId;

        public long Generate()
        {
            return ++_lastEntityId;
        }
    }
}