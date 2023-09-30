using System.Linq;
using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class AutoDestroyEntitySystem : EntitySystem, IEntitySystemRequireUpdate, IEntitySystemRequireEntityRemover
    {
        public IEntityRemover EntityRemover { get; set; }

        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<AutoDestroyEntityComponent>().ToArray();
            foreach (var entity in entities)
            {
                var component = entity.GetComponent<AutoDestroyEntityComponent>();

                component.Value -= deltaTime;
                if (component.Value > 0) continue;

                EntityRemover.Remove(entity.Id);
            }
        }
    }
}