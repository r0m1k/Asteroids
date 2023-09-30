using System.Linq;
using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PhysicsClearIsCollidedComponentSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<IsCollidedComponent>().ToArray();
            foreach (var entity in entities)
            {
                var component = entity.GetComponent<IsCollidedComponent>();
                component.OtherEntities.Clear();
            }
        }
    }
}