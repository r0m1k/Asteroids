using Services;

namespace Asteroids.Services.ECS
{
    public interface IEntityUniqueIdGeneratorService : IService
    {
        long Generate();
    }
}