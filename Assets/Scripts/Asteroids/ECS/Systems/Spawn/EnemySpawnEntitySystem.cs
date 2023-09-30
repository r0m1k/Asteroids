using Asteroids.ECS.Components;
using Asteroids.ECS.Entities;
using ECS;
using Infrastructure;
using System.Linq;
using Asteroids.Services;
using UnityEngine;

namespace Asteroids.ECS.Systems
{
    public abstract class EnemySpawnEntitySystem<T> : SpawnEntitySystem, IEntitySystemRequireUpdate, IEntitySystemRequireStart, IEntitySystemRequireEntityViewWorld where T : CooldownComponent, new()
    {
        protected abstract float CooldownTime { get; }
        protected abstract int MaximumSimultaneousEntities { get; }
        protected abstract float SafeSpawnRadiusAroundPlayer { get; }

        protected abstract bool GetSpawnParameters();
        protected abstract int CurrentSpawnedEntities();
        protected abstract int GenerateSpawnCount();
        protected abstract long SpawnEntity();

        public IReadOnlyEntityViewWorld ViewWorld { get; set; }

        protected EnemySpawnEntitySystem(IReadOnlyDataService dataService, IAsteroidsRulesService asteroidsRulesService, IRandomService randomService)
            : base(dataService, asteroidsRulesService, randomService)
        {
        }

        public void WorldStarted()
        {
            if (!GetSpawnParameters()) return;

            var worldEntity = World.FilterByComponents<IsWorldComponent>().FirstOrDefault();
            if (worldEntity == null) return;

            var spawn = worldEntity.AddComponentIfNotExists<T>();
            PrepareNextSpawn(spawn);
        }

        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<T>().ToArray();
            foreach (var entity in entities)
            {
                var cooldown = entity.GetComponent<T>();
                cooldown.Value -= deltaTime;

                if (cooldown.Value < 0) TrySpawn(cooldown);
            }
        }

        private void PrepareNextSpawn(T cooldown)
        {
            cooldown.Value = CooldownTime;
        }

        private void TrySpawn(T cooldown)
        {
            PrepareNextSpawn(cooldown);

            var currentCount = CurrentSpawnedEntities();

            var spawnCount = GenerateSpawnCount();
            spawnCount = spawnCount.ClampMax(MaximumSimultaneousEntities - currentCount);
            while (--spawnCount >= 0) DoSpawn();
        }

        protected void DoSpawn()
        {
            var id = SpawnEntity();
            var entity = World.Get(id);
            var entityView = ViewWorld.Get<EntityView>(id);

            var spawnPoint = GetSafeSpawnPoint();
            Positioning(entity, entityView, spawnPoint);
        }

        private Vector2 GetSafeSpawnPoint()
        {
            var playerEntity = World.FindFirst<PlayerShipEntity>();
            if (playerEntity == null) return Vector2.zero;

            var playerTransform = playerEntity.GetComponent<TransformComponent>();
            var playerPosition = playerTransform.Position;

            return _asteroidsRulesService.GetRandomSafePoint(playerPosition, SafeSpawnRadiusAroundPlayer);
        }

        private void Positioning(IEntity entity, EntityView entityView, Vector2 spawnPoint)
        {
            var transform = entity.GetComponent<TransformComponent>();
            transform.Position = spawnPoint;

            entityView.SetPosition(spawnPoint, 0);
        }
    }
}