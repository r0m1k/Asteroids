using UnityEngine;

namespace Infrastructure
{
    public static class NumericHelper
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static int ClampMax(this int value, int max)
        {
            if (value > max) value = max;
            return value;
        }

        public static float ClampMax(this float value, float max)
        {
            if (value > max) value = max;
            return value;
        }

        public static int ClampMin(this int value, int min)
        {
            if (value < min) value = min;
            return value;
        }

        public static float ClampMin(this float value, float min)
        {
            if (value < min) value = min;
            return value;
        }
    }
}