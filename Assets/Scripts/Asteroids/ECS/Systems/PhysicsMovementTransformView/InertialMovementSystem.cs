using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    // move entity with simple physics model
    public class InertialMovementSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<MovementComponent, TransformComponent>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<MovementComponent>();
                var transform = entity.GetComponent<TransformComponent>();

                // rotate
                transform.RotationDegreeAngle = CalculateRotation(fixedDeltaTime, transform.RotationDegreeAngle, movement.RotationDegreeSpeed);

                // move
                var acceleration = movement.Acceleration * transform.RotationDegreeAngle.AngleToVector2(); // accelerate in forward direction

                transform.Position = CalculatePosition(fixedDeltaTime, transform.Position, movement.Speed, acceleration);

                movement.Speed = CalculateSpeed(fixedDeltaTime, movement.Speed, acceleration);
            }
        }

        private float CalculateRotation(float deltaTime, float angleDegree, float angleDegreeSpeed)
        {
            angleDegree += angleDegreeSpeed * deltaTime;

            return angleDegree.ClampDegreeAngle();
        }

        private Vector2 CalculateSpeed(float deltaTime, Vector2 speed, Vector2 acceleration)
        {
            speed += acceleration * deltaTime;

            return speed;
        }

        private Vector2 CalculatePosition(float deltaTime, Vector2 pos, Vector2 speed, Vector2 acceleration)
        {
            // x = x + vt + at^2/2;

            pos += speed * deltaTime;
            pos += acceleration * deltaTime * deltaTime / 2;

            return pos;
        }
    }
}
