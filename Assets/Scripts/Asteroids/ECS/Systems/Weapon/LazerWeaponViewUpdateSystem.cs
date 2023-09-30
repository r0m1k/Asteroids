using Asteroids.ECS.Components;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public class LazerWeaponViewUpdateSystem : LazerWeaponUpdateSystem, IEntitySystemRequireUpdate, IEntitySystemRequireEntityRemover
    {
        public IEntityRemover EntityRemover { get; set; }

        public void Update(float deltaTime)
        {
            DoUpdate(deltaTime);
        }

        protected override void ProcessBulletPosition(IEntity entity, Vector2 position, float degreeAngle)
        {
            var transform = entity.GetComponent<TransformComponent>();
            transform.Position = position;
            transform.RotationDegreeAngle = degreeAngle;
        }

        protected override void ProcessError(IEntity entity)
        {
            EntityRemover.Remove(entity.Id);
        }
    }
}