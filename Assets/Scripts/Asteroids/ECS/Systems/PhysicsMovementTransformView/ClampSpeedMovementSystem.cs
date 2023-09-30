using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class ClampSpeedMovementSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<MovementComponent, MovementParametersComponent>();
            foreach (var entity in entities)
            {
                var parameters = entity.GetComponent<MovementParametersComponent>();
                if (!parameters.IsHasSpeedLimiter) continue;

                var movement = entity.GetComponent<MovementComponent>();
                var speedMagnitude = movement.Speed.magnitude;
                if (speedMagnitude > parameters.MaxSpeed)
                {
                    movement.Speed = parameters.MaxSpeed * movement.Speed.normalized;
                    movement.Acceleration = 0f;
                }
            }
        }
    }
}