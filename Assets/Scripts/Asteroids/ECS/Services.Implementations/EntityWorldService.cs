using Asteroids.ECS.Components;
using Asteroids.Services.ECS;
using ECS;
using Infrastructure;

namespace Asteroids.ECS.Services
{
    public class EntityWorldService : IEntityWorldService, IRequireCleanUp
    {
        private readonly IEntityFactoryService _entityFactoryService;

        public IEntityWorld World { get; }
        public IEntityViewWorld ViewWorld { get; }

        public EntityWorldService(IEntityFactoryService entityFactoryService)
        {
            _entityFactoryService = entityFactoryService;

            World = new EntityWorld();
            ViewWorld = new EntityViewWorld();
        }

        #region IEntityWorldService

        IReadOnlyEntityWorld IEntityWorldService.World => World;
        IReadOnlyEntityViewWorld IEntityWorldService.ViewWorld => ViewWorld;
        IEntityRemover IEntityWorldService.EntityRemover => this;

        public void CreateNewWorld()
        {
            var worldId = SpawnEmptyEntity();

            var world = World.Get(worldId);
            world.AddComponent<IsWorldComponent>();
        }

        public void CleanUp()
        {
            var entities = World.GetAll();
            foreach (var entity in entities) RemoveEntity(entity);
        }

        public long SpawnEmptyEntity()
        {
            return RegisterEntity(_entityFactoryService.SpawnEmptyEntity());
        }

        public long SpawnPlayer()
        {
            return GetMain(RegisterEntity(_entityFactoryService.SpawnPlayerShip()));
        }

        public long SpawnAlien()
        {
            return GetMain(RegisterEntity(_entityFactoryService.SpawnAlienShip()));
        }

        public long SpawnAsteroid()
        {
            return GetMain(RegisterEntity(_entityFactoryService.SpawnAsteroid()));
        }

        public long[] SpawnDebris(IEntity asteroid)
        {
            return GetAll(RegisterEntity(_entityFactoryService.SpawnDebris(asteroid)));
        }

        public long SpawnBullet(long weaponId)
        {
            var weapon = World.Get(weaponId);
            return GetMain(RegisterEntity(_entityFactoryService.SpawnBullet(weapon)));
        }

        public long SpawnEffect(EntityView effectViewPrefab)
        {
            return GetMain(RegisterEntity(_entityFactoryService.SpawnEffect(effectViewPrefab)));
        }

        public bool RemoveEntity(long id)
        {
            var entity = World.Get(id);
            return RemoveEntity(entity);
        }

        #endregion

        private EntityViewPairs RegisterEntity(EntityViewPairs pairs)
        {
            if (pairs.Main.Entity != null) RegisterEntity(pairs.Main);
            if (pairs.Childs == null) return pairs;

            foreach (var pair in pairs.Childs) RegisterEntity(pair);
            return pairs;
        }

        private long GetMain(EntityViewPairs pairs)
        {
            return pairs.Main.Entity?.Id ?? Constants.InvalidEntityId;
        }

        private long[] GetAll(EntityViewPairs pairs)
        {
            var ids = new long[(pairs.Main.Entity != null ? 1 : 0) + pairs.Childs?.Count ?? 0];

            for (var i = 0; i < (pairs.Childs?.Count ?? 0); ++i) ids[i] = pairs.Childs[i].Entity.Id;
            if (pairs.Main.Entity != null) ids[^1] = pairs.Main.Entity.Id;

            return ids;
        }

        private long RegisterEntity(EntityViewPair pair)
        {
            var id = pair.Entity.Id;
            World.Add(pair.Entity);
            if (pair.View != null) ViewWorld.Add(pair.View);

            return id;
        }

        public bool RemoveEntity(IEntity entity)
        {
            var view = ViewWorld.Get(entity.Id);
            _entityFactoryService.Destroy(entity, view);

            ViewWorld.Remove(entity.Id);
            return World.Remove(entity.Id);
        }

        bool IEntityRemover.Remove(long id)
        {
            return RemoveEntity(id);
        }
    }
}
