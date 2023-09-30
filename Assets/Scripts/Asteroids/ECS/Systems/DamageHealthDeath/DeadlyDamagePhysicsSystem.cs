using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class DeadlyDamagePhysicsSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsCollidedComponent, IsDeadlyDamageComponent>();
            foreach (var entity in entities)
            {
                var isCollided = entity.GetComponent<IsCollidedComponent>();
                foreach (var otherEntity in isCollided.OtherEntities)
                {
                    entity.AddComponentIfNotExists<IsDoDamageComponent>();
                    otherEntity.AddComponentIfNotExists<IsTakeDeadlyDamageComponent>();
                }
            }
        }
    }
}