using ECS;

namespace Asteroids.ECS.Components
{
    public class MovementParametersComponent : Component
    {
        public float ThrusterAcceleration;
        public float RotationSpeed;
        public float DecelerationByFriction;

        public float MaxSpeed;
        public bool IsHasSpeedLimiter => MaxSpeed > float.Epsilon;
    }
}