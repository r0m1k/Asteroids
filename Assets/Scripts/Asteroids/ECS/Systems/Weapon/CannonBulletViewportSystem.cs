using Asteroids.ECS.Components;
using ECS;
using System.Linq;
using Asteroids.Services;

namespace Asteroids.ECS.Systems
{
    public class CannonBulletViewportSystem : EntitySystem, IEntitySystemRequireFixedUpdate, IEntitySystemRequireEntityRemover
    {
        private readonly IAsteroidsRulesService _asteroidsRulesService;

        public IEntityRemover EntityRemover { get; set; }

        public CannonBulletViewportSystem(IAsteroidsRulesService asteroidsRulesService)
        {
            _asteroidsRulesService = asteroidsRulesService;
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsCannonBulletComponent, TransformComponent>().ToArray();
            foreach (var entity in entities)
            {
                var transform = entity.GetComponent<TransformComponent>();

                if (_asteroidsRulesService.InsideViewport(transform.Position)) continue;

                EntityRemover.Remove(entity.Id);
            }
        }
    }
}