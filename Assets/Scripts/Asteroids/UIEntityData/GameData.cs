using ECS;

namespace Asteroids.UIEntityData
{
    public class GameData
    {
        public long PlayerShipEntityId;
        public bool IsPlayerShipSpawned => PlayerShipEntityId != Constants.InvalidEntityId;

        public bool IsGamePlaying;
    }
}