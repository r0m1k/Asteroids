using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using Asteroids.Infrastructure;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class CannonWeaponFireSystem : WeaponFireSystem, IEntitySystemRequireEntityViewWorld
    {
        public IReadOnlyEntityViewWorld ViewWorld { get; set; }

        public CannonWeaponFireSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService)
            : base(WeaponType.Cannon, dataService, asteroidsRulesService, randomService)
        {
        }

        protected override void DoFire(IEntity weaponEntity, WeaponComponent weaponComponent)
        {
            var shipEntity = World.Get(weaponComponent.ShipEntityId);
            var shipTransform = shipEntity.GetComponent<TransformComponent>();
            var shipMovement = shipEntity.GetComponent<MovementComponent>();

            var shipView = ViewWorld.Get<ShipView>(weaponComponent.ShipEntityId);
            var firePoint = shipView.WeaponPivot.position;

            var bulletId = EntityWorldService.SpawnBullet(weaponEntity.Id);
            var bulletEntity = World.Get(bulletId);
            var bulletView = ViewWorld.Get<Physics2DView>(bulletId);

            var bulletBullet = bulletEntity.GetComponent<BulletComponent>();
            bulletBullet.WeaponEntityId = weaponEntity.Id;

            var bulletTransform = bulletEntity.GetComponent<TransformComponent>();
            bulletTransform.Position = firePoint;
            bulletTransform.RotationDegreeAngle = shipTransform.RotationDegreeAngle;

            var bulletMovement = bulletEntity.GetComponent<MovementComponent>();
            bulletMovement.Speed = shipMovement.Speed + weaponComponent.Parameters.BulletSpeed * shipTransform.RotationDegreeAngle.AngleToVector2();

            // ToDo: refactor to IsRequireTeleportMovementComponent
            bulletView.TeleportMovement(firePoint, shipTransform.RotationDegreeAngle);
        }
    }
}