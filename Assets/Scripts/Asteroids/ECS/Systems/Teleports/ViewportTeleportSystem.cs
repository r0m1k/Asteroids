using Asteroids.ECS.Components;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class ViewportTeleportSystem : ViewEntitySystem, IEntitySystemRequireFixedUpdate
    {
        private readonly IAsteroidsRulesService _asteroidsRulesService;

        public ViewportTeleportSystem(IAsteroidsRulesService asteroidsRulesService)
        {
            _asteroidsRulesService = asteroidsRulesService;
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<TransformComponent>();
            foreach (var entity in entities)
            {
                var transform = entity.GetComponent<TransformComponent>();

                if (_asteroidsRulesService.InsideViewport(transform.Position)) continue;

                transform.Position = _asteroidsRulesService.GetPointWithRespectToViewportTeleportation(transform.Position);

                var view = ViewWorld.Get<EntityView>(entity.Id);
                if (view != null) view.SetPosition(transform.Position, transform.RotationDegreeAngle);

                entity.AddComponentIfNotExists<IsTeleportedComponent>();
            }
        }
    }
}