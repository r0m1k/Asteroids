using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class PhysicsTransformUpdateViewSystem : ViewEntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<TransformComponent, IsPhysicComponent, IsViewComponent>();
            foreach (var entity in entities)
            {
                var transform = entity.GetComponent<TransformComponent>();
                var view = ViewWorld.Get<Physics2DView>(entity.Id);
                if (view == null) continue;

                view.PhysicsMovement(transform.Position, transform.RotationDegreeAngle);
            }
        }
    }
}