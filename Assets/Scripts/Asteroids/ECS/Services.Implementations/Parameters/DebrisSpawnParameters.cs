using Asteroids.ECS.Views;
using Infrastructure;
using UnityEngine;

namespace Asteroids.ECS.Services
{
    [CreateAssetMenu(fileName = "DebrisSpawn Parameters", menuName = "Asteroids Game/DebrisSpawn Parameters")]
    public class DebrisSpawnParameters : ScriptableObject
    {
        public DebrisView Prefab;
        public EffectView DeathEffectPrefab;

        [MinMaxRange(0.1f, 5f)]
        public MinMaxFloatRange SpeedModulation;

        [Range(0, 270)]
        public float SpeedDirectionModulation;

        [MinMaxRange(0.1f, 5f)]
        public MinMaxFloatRange RotationDegreeSpeedModulation;

        public bool RandomizeRotationDirection;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Health;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Damage;

        [Range(0, 10)]
        public int Score;
    }
}