using UnityEngine;

namespace Asteroids.ECS.Services
{
    [CreateAssetMenu(fileName = "PlayerMovement Parameters", menuName = "Asteroids Game/PlayerMovement Parameters")]
    public class PlayerMovementParameters : ScriptableObject
    {
        [Range(0, 500)]
        public float ThrusterAcceleration;

        [Range(0, 360)]
        public float RotationDirectionSpeed;

        [Range(0, 100)]
        public float DecelerationByFriction;
    }
}