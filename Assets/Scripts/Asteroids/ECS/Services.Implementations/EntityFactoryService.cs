using Asteroids.ECS.Components;
using Asteroids.ECS.Entities;
using Asteroids.ECS.Parameters;
using Asteroids.ECS.Views;
using Asteroids.Infrastructure;
using Asteroids.Services;
using Asteroids.Services.ECS;
using Asteroids.Services.Generator;
using ECS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.ECS.Services
{
    public class EntityFactoryService : IEntityFactoryService
    {
        private readonly IEntityUniqueIdGeneratorService _entityUniqueIdGenerator;
        private readonly IAssetService _assetService;
        private readonly IRandomService _randomService;
        private readonly IAsteroidGeneratorService _asteroidGenerator;

        private List<AsteroidSpec> _asteroidSpecs;

        private readonly EntityFactoryServiceParameters _parameters;

        public EntityFactoryService(EntityFactoryServiceParameters parameters, IEntityUniqueIdGeneratorService entityUniqueIdGenerator, IAssetService assetService, IRandomService randomService, IAsteroidGeneratorService asteroidGenerator)
        {
            _parameters = parameters;
            _entityUniqueIdGenerator = entityUniqueIdGenerator;
            _assetService = assetService;
            _randomService = randomService;
            _asteroidGenerator = asteroidGenerator;
        }

        #region IEntityFactoryService

        public EntityViewPairs SpawnPlayerShip()
        {
            var container = SpawnEntity<PlayerShipEntity>(_parameters.Player.SpawnParameters.ShipPrefab);
            ConfigurePlayerShip(container.Main);

            SpawnPlayerShipWeapons(ref container);

            return container;
        }

        public EntityViewPairs SpawnAlienShip()
        {
            var container = SpawnEntity<AlienShipEntity>(_parameters.Alien.SpawnParameters.ShipPrefab);
            ConfigureAlienShip(container.Main);

            return container;
        }

        public EntityViewPairs SpawnBullet(IEntity weapon)
        {
            var component = weapon.GetComponent<WeaponComponent>();
            if (component == null) return default;

            var weaponParameters = component.Parameters;
            if (weaponParameters == null) return default;

            var container = SpawnEntity(weaponParameters.EntityType, weaponParameters.BulletPrefab);
            ConfigureBullet(container.Main, weaponParameters);

            return container;
        }

        public EntityViewPairs SpawnEffect(IEntityView effectViewPrefab)
        {
            var container = SpawnEntity<EffectEntity>(effectViewPrefab);
            ConfigureEffect(container.Main);

            return container;
        }

        public EntityViewPair SpawnEmptyEntity()
        {
            return new EntityViewPair { Entity = CreateEntity<Entity>() };
        }

        public EntityViewPairs SpawnEmptyEntityWithView(EntityView view)
        {
            return SpawnEntity<Entity>(view);
        }

        public void Destroy(IEntity entity, IEntityView entityView)
        {
            if (entityView != null) _assetService.Destroy(entityView.GameObject);
        }

        public void GenerateAsteroids()
        {
            _asteroidSpecs = new List<AsteroidSpec>(_parameters.GenerateAsteroidVariants);
            for (var i = 0; i < _parameters.GenerateAsteroidVariants; ++i) _asteroidSpecs.Add(_asteroidGenerator.Generate());
        }

        public EntityViewPairs SpawnAsteroid()
        {
            var container = SpawnEntity<AsteroidEntity>(_parameters.AsteroidSpawn.Prefab);

            var asteroidSpecIndex = _randomService.GetInt(_asteroidSpecs.Count);
            ConfigureAsteroid(container.Main, asteroidSpecIndex);

            return container;
        }

        public EntityViewPairs SpawnDebris(IEntity asteroidEntity)
        {
            var asteroidComponent = asteroidEntity.GetComponent<AsteroidComponent>();
            var asteroidSpec = _asteroidSpecs[asteroidComponent.SpecIndex];

            var pairs = new EntityViewPairs()
            {
                Childs = new List<EntityViewPair>(asteroidSpec.VisualSpec.Debris.Length)
            };
            for (var i = 0; i < asteroidSpec.VisualSpec.Debris.Length; ++i)
            {
                var container = SpawnEntity<DebrisEntity>(_parameters.DebrisSpawn.Prefab);
                ConfigureDebris(asteroidEntity, container.Main, asteroidSpec, i);

                pairs.Childs.Add(container.Main);
            }

            return pairs;
        }

        #endregion

        private EntityViewPairs SpawnEntity<TEntity>(IEntityView view) where TEntity : IEntity, IEntitySetId, new()
        {
            return SpawnEntity<TEntity>(view.GameObject);
        }

        private EntityViewPairs SpawnEntity<TEntity>(GameObject prefab) where TEntity : IEntity, IEntitySetId, new()
        {
            var entity = CreateEntity<TEntity>();

            EntityView view = null;
            if (prefab) view = CreateView(prefab);
            view?.SetId(entity.Id);

            return CreateContainer(entity, view);
        }

        private EntityView CreateView(GameObject prefab)
        {
            var go = _assetService.Instantiate(prefab);
            var view = go.GetComponent<EntityView>();

            return view;
        }

        private TEntity CreateEntity<TEntity>() where TEntity : IEntity, IEntitySetId, new()
        {
            var entityId = GenerateEntityId();

            var entity = new TEntity();
            entity.SetId(entityId);

            return entity;
        }

        private EntityViewPairs SpawnEntity(string entityTypeName, EntityView view)
        {
            return SpawnEntity(entityTypeName, view?.gameObject);
        }

        private EntityViewPairs SpawnEntity(string entityTypeName, GameObject prefab)
        {
            var entityType = Type.GetType(entityTypeName);
            if (entityType == null) return default;

            var entity = Activator.CreateInstance(entityType) as IEntity;
            var entityId = GenerateEntityId();
            (entity as IEntitySetId).SetId(entityId);

            EntityView view = null;
            if (prefab) view = CreateView(prefab);
            view?.SetId(entity.Id);

            return CreateContainer(entity, view);
        }

        private EntityViewPairs SpawnEntity(Entity entity, GameObject prefab)
        {
            var entityId = GenerateEntityId();
            entity.SetId(entityId);

            EntityView view = null;
            if (prefab) view = CreateView(prefab);
            view?.SetId(entity.Id);

            return CreateContainer(entity, view);
        }

        private EntityViewPairs CreateContainer(IEntity entity, EntityView view)
        {
            return new EntityViewPairs
            {
                Main = new EntityViewPair {Entity = entity, View = view}
            };
        }

        private long GenerateEntityId()
        {
            return _entityUniqueIdGenerator.Generate();
        }

        #region Configure

        private void ConfigurePlayerShip(EntityViewPair pair)
        {
            var entity = pair.Entity;

            var health = entity.GetOrCreateComponent<HealthComponent>();
            health.Value = _parameters.Player.SpawnParameters.Health;

            var movement = entity.GetComponent<MovementParametersComponent>();
            movement.ThrusterAcceleration = _parameters.Player.MovementParameters.ThrusterAcceleration;
            movement.RotationSpeed = _parameters.Player.MovementParameters.RotationDirectionSpeed;
            movement.DecelerationByFriction = _parameters.Player.MovementParameters.DecelerationByFriction;

            var deathEffect = entity.GetComponent<DeathEffectComponent>();
            deathEffect.EffectPrefab = _parameters.Player.SpawnParameters.DeathEffectPrefab;
        }

        private void SpawnPlayerShipWeapons(ref EntityViewPairs container)
        {
            var playerAvailableWeapons = container.Main.Entity.GetOrCreateComponent<AvailableWeaponsComponent>();
            foreach (var weaponParameters in _parameters.Player.SpawnParameters.Weapons)
            {
                var weaponPair = SpawnEntity<WeaponEntity>((GameObject)null);

                container.Childs ??= new List<EntityViewPair>(_parameters.Player.SpawnParameters.Weapons.Length);
                container.Childs.Add(weaponPair.Main);

                var entity = weaponPair.Main.Entity;
                var weaponComponent = entity.GetComponent<WeaponComponent>();
                weaponComponent.ShipEntityId = container.Main.Entity.Id;
                weaponComponent.Parameters = weaponParameters;
                weaponComponent.Bullets = weaponParameters.MaxBullets;

                entity.AddComponentIfNotExists<IsPlayerComponent>();

                playerAvailableWeapons.Ids.Add(weaponPair.Main.Entity.Id);
            }
        }

        private void ConfigureAlienShip(EntityViewPair container)
        {
            var entity = container.Entity;

            var health = entity.GetComponent<HealthComponent>();
            health.Value = _randomService.GetInt(_parameters.Alien.SpawnParameters.Health);

            var doDamage = entity.GetComponent<DamageComponent>();
            doDamage.Value = _randomService.GetInt(_parameters.Alien.SpawnParameters.Damage);

            var movement = entity.GetComponent<MovementParametersComponent>();
            movement.ThrusterAcceleration = _parameters.Alien.MovementParameters.ThrusterAcceleration;
            movement.MaxSpeed = _parameters.Alien.MovementParameters.MaxSpeed;

            var parameters = entity.GetComponent<AlienParametersComponent>();
            parameters.PlayerShipSpeedToStopChasingThreshold = _parameters.Alien.MovementParameters.PlayerShipSpeedToStopChasingThreshold;

            var deathEffect = entity.GetComponent<DeathEffectComponent>();
            deathEffect.EffectPrefab = _parameters.Alien.SpawnParameters.DeathEffectPrefab;

            var score = entity.GetComponent<ScoreComponent>();
            score.Value = _parameters.Alien.SpawnParameters.Score;
        }

        // ToDo: move here position and speed for player/source of bullet
        // like as for asteroid
        private void ConfigureBullet(EntityViewPair pair, WeaponParameters weaponParameters)
        {
            var entity = pair.Entity;

            var bullet = entity.GetComponent<BulletComponent>();
            bullet.Parameters = weaponParameters;

            var damage = entity.GetComponent<DamageComponent>();
            damage.Value = weaponParameters.Damage;

            if (weaponParameters.BulletHasLifeTime)
            {
                var cooldown = entity.GetOrCreateComponent<BulletCooldown>();
                cooldown.Value = weaponParameters.BulletLifeTime;
            }
        }

        private void ConfigureEffect(EntityViewPair containerMain)
        {
            var effectView = containerMain.View as EffectView;
            if (effectView == null) return;
            if (!effectView.AutoDestroy) return;

            var autoDestroy = containerMain.Entity.AddComponent<AutoDestroyEntityComponent>();
            autoDestroy.Value = effectView.DestroyTimer;
        }

        private void ConfigureAsteroid(EntityViewPair containerMain, int specIndex)
        {
            var parameters = _parameters.AsteroidSpawn;
            var asteroidSpec = _asteroidSpecs[specIndex];

            var entity = containerMain.Entity;
            var view = containerMain.View as AsteroidView;

            #region entity
            var asteroid = entity.GetComponent<AsteroidComponent>();
            asteroid.SpecIndex = specIndex;

            var transform = entity.GetComponent<TransformComponent>();
            transform.RotationDegreeAngle = _randomService.GetFloat(360);

            var movement = entity.GetComponent<MovementComponent>();

            movement.Speed = _randomService.GetFloat(parameters.Speed) * transform.RotationDegreeAngle.AngleToVector2();
            movement.RotationDegreeSpeed = _randomService.GetFloat(parameters.RotationDegreeSpeed);

            var health = entity.GetComponent<HealthComponent>();
            health.Value = _randomService.GetInt(parameters.Health);

            var damage = entity.GetComponent<DamageComponent>();
            damage.Value = _randomService.GetInt(parameters.Damage);

            var score = entity.GetComponent<ScoreComponent>();
            score.Value = parameters.Score;

            if (parameters.DeathEffectPrefab) entity.AddComponentIfNotExists<DeathEffectComponent>().EffectPrefab = parameters.DeathEffectPrefab;

            #endregion

            #region view

            view.MeshFilter.mesh = asteroidSpec.VisualSpec.Mesh;
            view.PolygonCollider.SetPath(0, asteroidSpec.VisualSpec.ColliderPath);

            // ToDo: it wrong decision: move to system and then refactor to IsRequireTeleportMovementComponent
            view.TeleportMovement(transform.Position, transform.RotationDegreeAngle);

            #endregion
        }

        private void ConfigureDebris(IEntity asteroidEntity, EntityViewPair containerMain, AsteroidSpec asteroidSpec, int debrisSpecIndex)
        {
            var parameters = _parameters.DebrisSpawn;
            var debrisSpec = asteroidSpec.VisualSpec.Debris[debrisSpecIndex];

            var entity = containerMain.Entity;
            var view = containerMain.View as DebrisView;

            #region entity
            var asteroidTransform = asteroidEntity.GetComponent<TransformComponent>();
            var asteroidMovement = asteroidEntity.GetComponent<MovementComponent>();

            var asteroid = entity.GetComponent<AsteroidComponent>();
            asteroid.SpecIndex = debrisSpecIndex; // for debris it no sense

            var transform = entity.GetComponent<TransformComponent>();
            transform.Position = asteroidTransform.Position + debrisSpec.CenterPoint;
            transform.RotationDegreeAngle = asteroidTransform.RotationDegreeAngle;

            var movement = entity.GetComponent<MovementComponent>();

            movement.Speed = asteroidMovement.Speed * _randomService.GetFloat(parameters.SpeedModulation);
            movement.Speed = movement.Speed.Vector2Rotate(_randomService.GetFloat(-parameters.SpeedDirectionModulation, parameters.SpeedDirectionModulation));

            movement.RotationDegreeSpeed = asteroidMovement.RotationDegreeSpeed * _randomService.GetFloat(parameters.RotationDegreeSpeedModulation);
            if (parameters.RandomizeRotationDirection && _randomService.GetBool()) movement.RotationDegreeSpeed *= -1;

            var health = entity.GetComponent<HealthComponent>();
            health.Value = _randomService.GetInt(parameters.Health);

            var damage = entity.GetComponent<DamageComponent>();
            damage.Value = _randomService.GetInt(parameters.Damage);

            var score = entity.GetComponent<ScoreComponent>();
            score.Value = parameters.Score;

            if (parameters.DeathEffectPrefab) entity.AddComponentIfNotExists<DeathEffectComponent>().EffectPrefab = parameters.DeathEffectPrefab;

            #endregion

            #region view

            view.MeshFilter.mesh = debrisSpec.Mesh;
            view.PolygonCollider.SetPath(0, debrisSpec.ColliderPath);

            // ToDo: it wrong decision: move to system and then refactor to IsRequireTeleportMovementComponent
            view.TeleportMovement(transform.Position, transform.RotationDegreeAngle);

            #endregion
        }

        #endregion
    }
}