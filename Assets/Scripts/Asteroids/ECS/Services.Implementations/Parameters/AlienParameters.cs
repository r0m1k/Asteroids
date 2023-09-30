using System;
using Asteroids.ECS.Parameters;

namespace Asteroids.ECS.Services
{
    [Serializable]
    public class AlienParameters
    {
        public AlienSpawnParameters SpawnParameters;
        public AlienMovementParameters MovementParameters;
    }
}