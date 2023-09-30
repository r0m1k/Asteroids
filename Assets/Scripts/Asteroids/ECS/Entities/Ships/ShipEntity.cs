using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Entities
{
    public abstract class ShipEntity : Entity
    {
        protected override void AddComponents()
        {
            AddComponent<IsShipComponent>();

            AddComponent<IsPhysicComponent>();
            AddComponent<IsViewComponent>();
            
            AddComponent<MovementParametersComponent>();

            AddComponent<ShipInputComponent>();
            AddComponent<MovementComponent>();
            AddComponent<TransformComponent>();

            AddComponent<HealthComponent>();
            AddComponent<DeathEffectComponent>();

            AddComponent<IsHasThrusterComponent>();
        }
    }
}