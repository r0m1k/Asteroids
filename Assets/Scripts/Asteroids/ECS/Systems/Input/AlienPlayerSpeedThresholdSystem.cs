using System.Linq;
using Asteroids.ECS.Components;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public class AlienPlayerSpeedThresholdSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var playerEntity = World.FilterByComponents<IsPlayerShipComponent>().FirstOrDefault();
            var playerMovement = playerEntity?.GetComponent<MovementComponent>();
            var playerSpeed = playerMovement?.Speed.magnitude ?? 0;

            var entities = World.FilterByComponents<IsAlienShipComponent, ShipInputComponent, MovementComponent, AlienParametersComponent>();
            foreach (var entity in entities)
            {
                var parameters = entity.GetComponent<AlienParametersComponent>();
                var allowAlienMovementThreshold = playerSpeed <= parameters.PlayerShipSpeedToStopChasingThreshold;
                if (allowAlienMovementThreshold) continue;

                var input = entity.GetComponent<ShipInputComponent>();
                var movement = entity.GetComponent<MovementComponent>();

                input.Thruster = 0;
                movement.Speed = Vector2.zero;
            }
        }
    }
}