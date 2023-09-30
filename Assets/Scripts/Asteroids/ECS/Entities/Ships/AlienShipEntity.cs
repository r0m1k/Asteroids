using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Entities
{
    public class AlienShipEntity : ShipEntity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<IsAlienComponent>();
            AddComponent<IsAlienShipComponent>();

            AddComponent<AlienParametersComponent>();

            AddComponent<DamageComponent>();

            AddComponent<ScoreComponent>();
        }
    }
}