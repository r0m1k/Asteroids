using System.Linq;
using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PhysicsRemoveIsCollidedComponentSystem : EntitySystem, IEntitySystemRequireUpdate
    {
        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsCollidedComponent>().ToArray();
            foreach (var entity in entities) entity.RemoveComponent<IsCollidedComponent>();
        }
    }
}