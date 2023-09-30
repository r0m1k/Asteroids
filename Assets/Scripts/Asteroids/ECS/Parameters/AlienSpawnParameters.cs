using Asteroids.ECS.Views;
using Infrastructure;
using UnityEngine;

namespace Asteroids.ECS.Parameters
{
    [CreateAssetMenu(fileName = "AlienSpawn Parameters", menuName = "Asteroids Game/AlienSpawn Parameters")]
    public class AlienSpawnParameters : ScriptableObject
    {
        public AlienShipView ShipPrefab;
        public EffectView DeathEffectPrefab;

        [MinMaxRange(0, 60)]
        public MinMaxIntRange SpawnCooldown;

        [MinMaxRange(0, 5)]
        public MinMaxIntRange SpawnCount;

        [Range(1, 15)]
        public int MaximumSimultaneousCount;

        [Range(0, 200)]
        public int SafeSpawnRadiusAroundPlayer;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Health;

        [MinMaxRange(1, 10)]
        public MinMaxIntRange Damage;

        [Range(0, 10)]
        public int Score;
    }
}