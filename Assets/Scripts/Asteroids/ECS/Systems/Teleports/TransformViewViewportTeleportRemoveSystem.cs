using System.Linq;
using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class TransformViewViewportTeleportRemoveSystem : EntitySystem, IEntitySystemRequireLateUpdate
    {
        public void LateUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<IsTeleportedComponent>().ToArray();
            foreach (var entity in entities) entity.RemoveComponent<IsTeleportedComponent>();
        }
    }
}