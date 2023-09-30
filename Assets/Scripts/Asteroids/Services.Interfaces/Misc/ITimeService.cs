using Services;

namespace Asteroids.Services
{
    public interface ITimeService : IService
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }

        float Time { get; }
        int Frame { get; }
    }
}
