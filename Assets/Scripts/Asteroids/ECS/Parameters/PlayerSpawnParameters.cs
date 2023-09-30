using Asteroids.ECS.Views;
using Asteroids.Services.ECS;
using UnityEngine;

namespace Asteroids.ECS.Parameters
{
    [CreateAssetMenu(fileName = "PlayerSpawn Parameters", menuName = "Asteroids Game/PlayerSpawn Parameters")]
    public class PlayerSpawnParameters : ScriptableObject
    {
        public PlayerShipView ShipPrefab;
        public EffectView DeathEffectPrefab;

        [Range(1, 10)]
        public int Health;

        public WeaponParameters[] Weapons;
    }
}