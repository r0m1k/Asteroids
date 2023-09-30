using System.Linq;
using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using ECS;

namespace Asteroids.ECS.Systems
{
    // read Unity physics events into entity
    // ToDo: check if trigger come multiple times per
    public class PhysicsCollectCollisionsSystem : ViewEntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsPhysicComponent, IsViewComponent, TransformComponent>();
            foreach (var entity in entities) CollectCollisions(entity);
        }

        private void CollectCollisions(IEntity entity)
        {
            var entityView = ViewWorld.Get<Physics2DView>(entity.Id);
            if (entityView == null) return;

            var collidedViews = entityView.GetCollisions();
            if ((collidedViews?.Length ?? 0) == 0) return;

            var component = entity.GetOrCreateComponent<IsCollidedComponent>();
            component.OtherEntities.AddRange(collidedViews.Select(view => World.Get(view.EntityId)));
        }
    }
}
