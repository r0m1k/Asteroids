using Infrastructure;
using UnityEngine;

namespace Asteroids.Services
{
    public class RandomService : IRandomService
    {
        private const float BoolTrueThresholdValue = 0.5f;

        public RandomService(ITimeService timeService)
        {
            var seedValue = (int) (100000 * GetFloat());
            Random.InitState(seedValue);
        }

        public bool GetBool()
        {
            return GetBool(BoolTrueThresholdValue);
        }

        public bool GetBool(float thresholdValue)
        {
            return GetFloat() > thresholdValue;
        }

        public int GetInt(int maxValue)
        {
            return GetInt(0, maxValue);
        }

        public int GetInt(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public int GetInt(MinMaxIntRange minMaxRange)
        {
            return GetInt(minMaxRange.Min, minMaxRange.Max);
        }

        public float GetFloat()
        {
            return Random.value;
        }

        public float GetFloat(int maxValue)
        {
            return maxValue * GetFloat();
        }

        public float GetFloat(int minValue, int maxValue)
        {
            var range = maxValue - minValue;
            return minValue + range * GetFloat();
        }

        public float GetFloat(float minValue, float maxValue)
        {
            var range = maxValue - minValue;
            return minValue + range * GetFloat();
        }

        public float GetFloat(MinMaxIntRange minMaxRange)
        {
            return GetFloat(minMaxRange.Min, minMaxRange.Max);
        }

        public float GetFloat(MinMaxFloatRange minMaxRange)
        {
            return GetFloat(minMaxRange.Min, minMaxRange.Max);
        }
    }
}
