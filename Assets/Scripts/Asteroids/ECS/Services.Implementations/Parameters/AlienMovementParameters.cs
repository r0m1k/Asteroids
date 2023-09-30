using UnityEngine;

namespace Asteroids.ECS.Services
{
    [CreateAssetMenu(fileName = "AlienMovement Parameters", menuName = "Asteroids Game/AlienMovement Parameters")]
    public class AlienMovementParameters : ScriptableObject
    {
        [Range(0, 500)]
        public float ThrusterAcceleration;

        [Range(0, 500)]
        public float MaxSpeed;

        [Range(0, 2000)]
        public float PlayerShipSpeedToStopChasingThreshold;
    }
}