using Services;

namespace Asteroids.Services.Generator
{
    public interface IAsteroidVisualGeneratorService : IService
    {
        AsteroidVisualSpec Generate(AsteroidGeometrySpec geometrySpec);
    }
}