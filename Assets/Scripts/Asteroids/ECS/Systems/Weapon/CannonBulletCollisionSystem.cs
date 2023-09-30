using System.Linq;
using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class CannonBulletCollisionSystem : EntitySystem, IEntitySystemRequireFixedUpdate, IEntitySystemRequireEntityRemover
    {
        public IEntityRemover EntityRemover { get; set; }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsCollidedComponent, IsCannonBulletComponent>().ToArray();
            foreach (var entity in entities) EntityRemover.Remove(entity.Id);
        }
    }
}