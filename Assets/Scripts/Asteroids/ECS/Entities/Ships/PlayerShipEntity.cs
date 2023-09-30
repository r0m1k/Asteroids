using Asteroids.ECS.Components;

namespace Asteroids.ECS.Entities
{
    public class PlayerShipEntity : ShipEntity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<IsPlayerComponent>();
            AddComponent<IsPlayerShipComponent>();

            AddComponent<PlayerParametersComponent>();
            AddComponent<AvailableWeaponsComponent>();
        }
    }
}