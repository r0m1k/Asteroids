using System.Linq;
using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public abstract class LazerWeaponUpdateSystem : ViewEntitySystem
    {
        protected void DoUpdate(float deltaTime)
        {
            var entities = World.FilterByComponents<IsLazerBulletComponent>().ToArray();
            foreach (var entity in entities)
            {
                var bulletTransformations = GetLazerBulletTransformation(entity);

                if (bulletTransformations.shipExists)
                {
                    ProcessBulletPosition(entity, bulletTransformations.position, bulletTransformations.degreeAngle);
                }
                else
                {
                    ProcessError(entity);
                }
            }
        }

        protected abstract void ProcessBulletPosition(IEntity entity, Vector2 position, float degreeAngle);
        protected abstract void ProcessError(IEntity entity);

        protected (bool shipExists, Vector2 position, float degreeAngle) GetLazerBulletTransformation(IEntity bulletEntity)
        {
            var bullet = bulletEntity.GetComponent<BulletComponent>();

            var weaponEntity = World.Get(bullet.WeaponEntityId);
            if (weaponEntity == null) return (false, default, default);

            var weapon = weaponEntity.GetComponent<WeaponComponent>();

            var shipEntity = World.Get(weapon.ShipEntityId);
            if (shipEntity == null) return (false, default, default);

            var shipTransform = shipEntity.GetComponent<TransformComponent>();

            var shipView = ViewWorld.Get<ShipView>(weapon.ShipEntityId);
            var firePoint = shipView.WeaponPivot.position;

            return (true, firePoint, shipTransform.RotationDegreeAngle);
        }
    }
}