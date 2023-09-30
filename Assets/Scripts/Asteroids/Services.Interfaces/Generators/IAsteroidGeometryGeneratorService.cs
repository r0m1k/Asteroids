using Services;

namespace Asteroids.Services.Generator
{
    public interface IAsteroidGeometryGeneratorService : IService
    {
        AsteroidGeometrySpec Generate();
    }
}