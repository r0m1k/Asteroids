using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    // update Unity GameObjects, but not Physics
    public class NonPhysicsTransformViewUpdateSystem : ViewEntitySystem, IEntitySystemRequireUpdate
    {
        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<TransformComponent, IsViewComponent>().Excl<IsPhysicComponent>();
            foreach (var entity in entities)
            {
                var transform = entity.GetComponent<TransformComponent>();
                var view = ViewWorld.Get(entity.Id);

                view.SetPosition(transform.Position, transform.RotationDegreeAngle);
            }
        }
    }

    // update Unity GameObjects, but only Physics!
}
