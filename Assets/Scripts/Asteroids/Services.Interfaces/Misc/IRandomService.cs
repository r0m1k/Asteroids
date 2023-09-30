using Infrastructure;
using Services;

namespace Asteroids.Services
{
    // not use overload to make call strict
    public interface IRandomService : IService
    {
        bool GetBool();
        bool GetBool(float thresholdValue);

        int GetInt(int maxValue);
        int GetInt(int minValue, int maxValue);
        int GetInt(MinMaxIntRange minMaxRange);

        float GetFloat();
        float GetFloat(int maxValue);
        float GetFloat(int minValue, int maxValue);
        float GetFloat(float minValue, float maxValue);
        float GetFloat(MinMaxIntRange minMaxRange);
        float GetFloat(MinMaxFloatRange minMaxRange);
    }
}
