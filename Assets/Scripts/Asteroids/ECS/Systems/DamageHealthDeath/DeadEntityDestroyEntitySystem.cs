using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using ECS;
using System.Linq;

namespace Asteroids.ECS.Systems
{
    public class DeadEntityDestroyEntitySystem : ViewEntitySystem, IEntitySystemRequireFixedUpdate, IEntitySystemRequireWorldService
    {
        public IEntityWorldService EntityWorldService { get; set; }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var entities = World.FilterByComponents<IsDeadComponent>().ToArray();
            foreach (var entity in entities)
            {
                TrySpawnDeathEffect(entity);
                EntityWorldService.RemoveEntity(entity.Id);
            }
        }

        private void TrySpawnDeathEffect(IEntity entity)
        {
            var deathEffect = entity.GetComponent<DeathEffectComponent>();
            if (deathEffect == null) return;

            var transform = entity.GetComponent<TransformComponent>();
            if (transform == null) return;

            var effectId = EntityWorldService.SpawnEffect(deathEffect.EffectPrefab);
            var effectView = ViewWorld.Get(effectId);

            effectView.SetPosition(transform.Position, transform.RotationDegreeAngle);
        }
    }
}