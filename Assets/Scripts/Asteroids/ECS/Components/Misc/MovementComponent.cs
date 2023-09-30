using UnityEngine;
using Component = ECS.Component;

namespace Asteroids.ECS.Components
{
    public class MovementComponent : Component
    {
        public Vector2 Speed;

        public float Acceleration;
        public bool IsAccelerating => Mathf.Abs(Acceleration) > float.Epsilon;

        public float RotationDegreeSpeed;
    }
}