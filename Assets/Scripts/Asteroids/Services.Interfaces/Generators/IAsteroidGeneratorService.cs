using Services;

namespace Asteroids.Services.Generator
{
    public interface IAsteroidGeneratorService : IService
    {
        AsteroidSpec Generate();
    }
}