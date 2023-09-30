using Asteroids.ECS.Parameters;
using Asteroids.Services.ECS;
using UnityEngine;

namespace Asteroids.ECS.Services
{
    [CreateAssetMenu(fileName = "EntityFactoryService Parameters", menuName = "Asteroids Game/EntityFactoryService Parameters")]
    public class EntityFactoryServiceParameters : ScriptableObject
    {
        public PlayerParameters Player;
        public AlienParameters Alien;

        [Range(1, 50)]
        public int GenerateAsteroidVariants;

        public AsteroidSpawnParameters AsteroidSpawn;
        public DebrisSpawnParameters DebrisSpawn;
    }
}