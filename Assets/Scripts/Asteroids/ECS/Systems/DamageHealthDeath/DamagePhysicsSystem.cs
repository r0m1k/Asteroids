using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class DamagePhysicsSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsCollidedComponent, DamageComponent>();
            foreach (var entity in entities)
            {
                var doDamage = entity.GetComponent<DamageComponent>();

                var isCollided = entity.GetComponent<IsCollidedComponent>();
                foreach (var otherEntity in isCollided.OtherEntities)
                {
                    entity.AddComponentIfNotExists<IsDoDamageComponent>();

                    var isTakeDamage = otherEntity.AddComponentIfNotExists<IsTakeDamageComponent>();
                    isTakeDamage.Values.Add(doDamage.Value);
                }
            }
        }
    }
}