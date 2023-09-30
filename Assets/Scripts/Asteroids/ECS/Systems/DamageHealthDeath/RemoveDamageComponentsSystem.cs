using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class RemoveDamageComponentsSystem : EntitySystem, IEntitySystemRequireLateUpdate
    {
        public void LateUpdate(float deltaTime)
        {
            // ToDo: need to separate but for now all in one place in more understandable and transparent
            RemoveComponent<IsDoDamageComponent>();
            RemoveComponent<IsTakeDamageComponent>();
            RemoveComponent<IsTakeDeadlyDamageComponent>();
        }

        private void RemoveComponent<T>() where T : Component
        {
            var entities = World.FilterByComponents<T>();
            foreach (var entity in entities) entity.RemoveComponent<T>();
        }
    }
}