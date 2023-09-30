using System.Diagnostics;
using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    // translate inputs into PlayerInputComponent
    public class PlayerShipInputSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        private readonly IPlayerShipInput _playerInput;

        public PlayerShipInputSystem(IPlayerShipInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsPlayerComponent, ShipInputComponent>();
            foreach (var entity in entities)
            {
                var input = entity.GetComponent<ShipInputComponent>();

                input.Rotation = _playerInput.GetRotation().InputAxisRotationToWorldRotation(); // inverted because in mind left is ccw, but in game is positive angle
                input.Thruster = _playerInput.GetThruster();
            }
        }
    }
}
