using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class AlienInputSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsAlienShipComponent, ShipInputComponent>();
            foreach (var entity in entities)
            {
                var input = entity.GetComponent<ShipInputComponent>();

                input.Thruster = 1f;
                input.Rotation = 0;
            }
        }
    }
}