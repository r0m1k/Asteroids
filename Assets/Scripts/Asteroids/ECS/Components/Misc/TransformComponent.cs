using UnityEngine;
using Component = ECS.Component;

namespace Asteroids.ECS.Components
{
    public class TransformComponent : Component
    {
        public Vector2 Position;
        public float RotationDegreeAngle;
    }
}