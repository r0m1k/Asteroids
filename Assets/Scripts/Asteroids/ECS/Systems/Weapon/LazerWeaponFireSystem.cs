using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using Asteroids.Infrastructure;
using Asteroids.Services;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class LazerWeaponFireSystem : WeaponFireSystem, IEntitySystemRequireEntityViewWorld
    {
        public IReadOnlyEntityViewWorld ViewWorld { get; set; }

        public LazerWeaponFireSystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService)
            : base(WeaponType.Laser, dataService, asteroidsRulesService, randomService)
        {
        }

        protected override void DoFire(IEntity weaponEntity, WeaponComponent weaponComponent)
        {
            var shipEntity = World.Get(weaponComponent.ShipEntityId);
            var shipTransform = shipEntity.GetComponent<TransformComponent>();

            var shipView = ViewWorld.Get<ShipView>(weaponComponent.ShipEntityId);
            var firePoint = shipView.WeaponPivot.position;

            var bulletId = EntityWorldService.SpawnBullet(weaponEntity.Id);
            var bulletEntity = World.Get(bulletId);
            var bulletView = ViewWorld.Get<LazerView>(bulletId);

            var bulletBullet = bulletEntity.GetComponent<BulletComponent>();
            bulletBullet.WeaponEntityId = weaponEntity.Id;

            bulletView.SetPosition(firePoint, shipTransform.RotationDegreeAngle);
            bulletView.SetBeamLength(1.2f * _asteroidsRulesService.GetMinLengthToCrossViewport());
        }
    }
}