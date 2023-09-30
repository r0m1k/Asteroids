using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class ShipInputToMovementSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<ShipInputComponent, MovementComponent, MovementParametersComponent>();
            foreach (var entity in entities)
            {
                var input = entity.GetComponent<ShipInputComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var movementParameters = entity.GetComponent<MovementParametersComponent>();

                movement.Acceleration = input.Thruster * movementParameters.ThrusterAcceleration;
                movement.RotationDegreeSpeed = movementParameters.RotationSpeed * input.Rotation;
            }
        }
    }
}