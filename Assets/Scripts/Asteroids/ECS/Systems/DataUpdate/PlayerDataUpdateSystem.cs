using Asteroids.ECS.Components;
using Asteroids.ECS.Entities;
using Asteroids.Services;
using Asteroids.UIEntityData;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PlayerDataUpdateSystem : EntitySystem, IEntitySystemRequireUpdate, IEntitySystemRequireStart
    {
        private readonly IDataService _dataService;

        private IDataWriter<PlayerShipData> _playerShipData;

        public PlayerDataUpdateSystem(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void WorldStarted()
        {
            _playerShipData = _dataService.GetFirstOrCreate<PlayerShipData>();
        }

        public void Update(float deltaTime)
        {
            var playerShip = World.FindFirst<PlayerShipEntity>();
            if (playerShip == null) return;

            var transform = playerShip.GetComponent<TransformComponent>();
            var movement = playerShip.GetComponent<MovementComponent>();

            _playerShipData.Data.Position = transform.Position;
            _playerShipData.Data.Speed = movement.Speed;
            _playerShipData.Data.Acceleration = movement.Acceleration;

            _playerShipData.Data.RotationDegreeAngle = transform.RotationDegreeAngle;
            _playerShipData.Data.RotationDegreeSpeed = movement.RotationDegreeSpeed;

            _playerShipData.Notify();
        }
    }
}