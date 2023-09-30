using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class TakeDamageUpdateHealthSystem : EntitySystem, IEntitySystemRequireFixedUpdate, IEntitySystemRequireUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            DoUpdate();
        }

        public void Update(float deltaTime)
        {
            DoUpdate();
        }

        private void DoUpdate()
        {
            var entities = World.FilterByComponents<IsTakeDamageComponent>();
            foreach (var entity in entities)
            {
                var takeDamage = entity.GetComponent<IsTakeDamageComponent>();

                var health = entity.GetComponent<HealthComponent>();
                if (health == null) continue;

                health.Value -= takeDamage.Value;
            }
        }
    }
}