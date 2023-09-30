using System.Linq;
using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class BulletLifeTypeSystem : EntitySystem, IEntitySystemRequireLateUpdate, IEntitySystemRequireEntityRemover
    {
        public IEntityRemover EntityRemover { get; set; }

        public void LateUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<BulletCooldown>().ToArray();
            foreach (var entity in entities)
            {
                var cooldown = entity.GetComponent<BulletCooldown>();
                cooldown.Value -= deltaTime;
                if (cooldown.Value > 0) continue;

                EntityRemover.Remove(entity.Id);
            }
        }
    }
}