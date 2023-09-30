using UnityEngine;

namespace Asteroids.Infrastructure
{
    public static class MovementHelper
    {
        public static float InputAxisRotationToWorldRotation(this float rotationDirection)
        {
            return -rotationDirection;
        }

        public static float ClampDegreeAngle(this float angle)
        {
            while (angle < 0) angle += 360;
            while (angle >= 360) angle -= 360;

            return angle;
        }

        public static Vector2 AngleToVector2(this float degreeAngle)
        {
            var rotation = Quaternion.Euler(0, 0, degreeAngle);
            var rotatedVector = rotation * Vector3.up;

            return new Vector2(rotatedVector.x, rotatedVector.y);
        }

        public static float Vector2ToAngle(this Vector2 direction)
        {
            return Vector2.SignedAngle(Vector2.up, direction);
        }

        public static Vector2 Vector2Rotate(this Vector2 vector, float degreeAngle)
        {
            var rotation = Quaternion.Euler(0, 0, degreeAngle);
            var rotatedVector = rotation * vector;

            return new Vector2(rotatedVector.x, rotatedVector.y);
        }
    }
}