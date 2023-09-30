using System;
using Asteroids.ECS.Parameters;

namespace Asteroids.ECS.Services
{
    [Serializable]
    public class PlayerParameters
    {
        public PlayerSpawnParameters SpawnParameters;
        public PlayerMovementParameters MovementParameters;
    }
}