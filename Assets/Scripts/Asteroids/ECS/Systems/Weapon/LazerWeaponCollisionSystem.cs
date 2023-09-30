using Asteroids.ECS.Components;
using Asteroids.Infrastructure;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public class LazerWeaponCollisionSystem : LazerWeaponUpdateSystem, IEntitySystemRequireFixedUpdate
    {
        public void FixedUpdate(float fixedDeltaTime)
        {
            DoUpdate(fixedDeltaTime);
        }

        protected override void ProcessBulletPosition(IEntity entity, Vector2 position, float degreeAngle)
        {
            var entityView = ViewWorld.Get(entity.Id);
            var viewLayer = entityView.GameObject.layer;
            var collisionLayerMask = Physics2D.GetLayerCollisionMask(viewLayer);

            IsCollidedComponent isCollided = null;

            var direction = degreeAngle.AngleToVector2();
            var hits = Physics2D.RaycastAll(position, direction, Mathf.Infinity, collisionLayerMask);
            foreach (var hit in hits)
            {
                var collidedView = hit.transform.GetComponent<EntityView>();
                if (collidedView == null) continue;

                var otherEntity = World.Get(collidedView.EntityId);
                if (otherEntity == null) continue;

                if (isCollided == null) isCollided = entity.GetOrCreateComponent<IsCollidedComponent>();
                isCollided.OtherEntities.Add(otherEntity);
            }
        }

        protected override void ProcessError(IEntity entity) { }
    }
}