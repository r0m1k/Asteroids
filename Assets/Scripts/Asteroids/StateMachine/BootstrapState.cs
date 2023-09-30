using Asteroids.ECS.Systems;
using Asteroids.Infrastructure;
using Asteroids.Services;
using Asteroids.Services.ECS;
using Services;
using StateMachine;

namespace Asteroids.StateMachine
{
    public class BootstrapState : State
    {
        private readonly IServiceProvider _services;

        public BootstrapState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
        }

        public override void Enter()
        {
            ShowLoadScreen();

            ConfigureEntitySystems();
            CreateHUD();

            _stateMachine.Enter<AsteroidGeneratorState>();
        }

        public override void Exit() { }

        private void ConfigureEntitySystems()
        {
            var dataService = _services.Get<IDataService>();
            var randomService = _services.Get<IRandomService>();
            var asteroidsRulesService = _services.Get<IAsteroidsRulesService>();

            var inputService = _services.Get<IInputService>();
            var playerInput = inputService.GetPlayerShipInput();

            // because no dependencies order is important for nice (and often correct) flow!
            var entitySystemService = _services.Get<IEntitySystemService>();
            entitySystemService
                // spawns
                .AddSystem(new PlayerSpawnSystem(dataService, asteroidsRulesService, randomService))
                .AddSystem(new AlienSpawnSystem(dataService, asteroidsRulesService, randomService))
                .AddSystem(new AsteroidSpawnSystem(dataService, asteroidsRulesService, randomService))

                // fixed update: physics collisions
                .AddSystem<PhysicsCollectCollisionsSystem>()
                .AddSystem<LazerWeaponCollisionSystem>()
                .AddSystem<DamagePhysicsSystem>()
                .AddSystem<DeadlyDamagePhysicsSystem>()
                .AddSystem<CannonBulletCollisionSystem>()
                // fixed update: death
                .AddSystem<TakeDamageUpdateHealthSystem>()
                .AddSystem<CheckHealthEntitySystem>()
                .AddSystem<CheckHealthDeadlyDamageEntitySystem>()
                .AddSystem(new DeadEntityScoreSystem(dataService))
                .AddSystem(new DeadAsteroidBreakToDebrisSystem(dataService, asteroidsRulesService, randomService))
                .AddSystem<DeadEntityDestroyEntitySystem>()

                // fixed update: physics movement
                // alien
                .AddSystem<AlienPlayerSpeedThresholdSystem>()
                .AddSystem(new AlienTargetMovementSystem(asteroidsRulesService))
                .AddSystem<ClampSpeedMovementSystem>()
                // common
                .AddSystem<InertialMovementSystem>()
                .AddSystem(new CannonBulletViewportSystem(asteroidsRulesService))
                .AddSystem(new ViewportTeleportSystem(asteroidsRulesService))
                .AddSystem<PhysicsTransformUpdateViewSystem>()
                // fixed update: clear components
                .AddSystem<PhysicsClearIsCollidedComponentSystem>()

                // update: clear fixed update
                .AddSystem<PhysicsRemoveIsCollidedComponentSystem>()
                // update: common
                .AddSystem(new PlayerShipInputSystem(playerInput))
                .AddSystem<AlienInputSystem>()
                .AddSystem<ShipInputToMovementSystem>()
                // update: weapon
                .AddSystem(new PlayerWeaponInputSystem(playerInput))
                .AddSystem(new CannonWeaponFireSystem(dataService, asteroidsRulesService, randomService))
                .AddSystem(new LazerWeaponFireSystem(dataService, asteroidsRulesService, randomService))
                // update: views
                .AddSystem<LazerWeaponViewUpdateSystem>()
                .AddSystem<NonPhysicsTransformViewUpdateSystem>()
                .AddSystem<AutoDestroyEntitySystem>()
                .AddSystem<ShowShipThrusterOnAccelerateSystem>()
                // update: misc
                .AddSystem(new PlayerDataUpdateSystem(dataService))
                .AddSystem(new PlayerWeaponDataUpdateSystem(WeaponType.Laser, dataService))
                .AddSystem(new GameDataUpdateSystem(dataService))

                // late update: weapons
                .AddSystem<BulletLifeTypeSystem>()
                .AddSystem<WeaponRechargeCooldownUpdateSystem>()
                .AddSystem<WeaponRechargeUpdateSystem>()
                .AddSystem<WeaponFireCooldownUpdateSystem>()

                // late update: remove components
                .AddSystem(new TransformViewViewportTeleportRemoveSystem())
                .AddSystem<RemoveDamageComponentsSystem>()
                ;
        }

        private void CreateHUD()
        {
            var uiService = _services.Get<IUIService>();
            uiService.CreateHud();
        }

        private void ShowLoadScreen()
        {
            var loadScreen = _services.Get<ILoadScreenService>();
            loadScreen.ShowImmediately();
        }
    }
}