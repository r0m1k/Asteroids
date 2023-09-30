using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using ECS;
using System.Linq;
using Asteroids.Services;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public class AlienTargetMovementSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        private readonly IAsteroidsRulesService _asteroidsRulesService;

        public AlienTargetMovementSystem(IAsteroidsRulesService asteroidsRulesService)
        {
            _asteroidsRulesService = asteroidsRulesService;
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var playerEntity = World.FilterByComponents<IsPlayerShipComponent, TransformComponent>().FirstOrDefault();
            if (playerEntity == null) return;

            var playerTransform = playerEntity.GetComponent<TransformComponent>();

            var entities = World.FilterByComponents<IsAlienShipComponent, MovementComponent, TransformComponent>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<MovementComponent>();
                var transform = entity.GetComponent<TransformComponent>();

                var fakePosition = _asteroidsRulesService.GetFakeClosesPositionWithTeleportationRespect(transform.Position, playerTransform.Position);
                var directionToPlayer = (fakePosition - transform.Position).normalized;
                var speedMagnitude = movement.Speed.magnitude;

                // change speed vector to target
                movement.Speed = speedMagnitude * directionToPlayer;
                // change look direction to target: do it in NonInertialMovementSystem
                transform.RotationDegreeAngle = directionToPlayer.Vector2ToAngle();
            }
        }

        
    }
}