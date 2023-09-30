using Asteroids.ECS.Views;
using Infrastructure;
using UnityEngine;

namespace Asteroids.ECS.Parameters
{
    [CreateAssetMenu(fileName = "AsteroidSpawn Parameters", menuName = "Asteroids Game/AsteroidSpawn Parameters")]
    public class AsteroidSpawnParameters : ScriptableObject
    {
        public AsteroidView Prefab;
        public EffectView DeathEffectPrefab;

        [MinMaxRange(0, 20)]
        public MinMaxIntRange SpawnCooldown;

        [MinMaxRange(0, 5)]
        public MinMaxIntRange SpawnCount;

        [Range(1, 30)]
        public int MaximumSimultaneousCount;

        [Range(0, 200)]
        public float SafeSpawnRadiusAroundPlayer;

        [MinMaxRange(0, 400)]
        public MinMaxFloatRange Speed;

        [MinMaxRange(0, 360)]
        public MinMaxIntRange RotationDegreeSpeed;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Health;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Damage;

        [Range(0, 10)]
        public int Score;
    }
}