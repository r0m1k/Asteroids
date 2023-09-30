using Asteroids.Services;
using Asteroids.Services.ECS;
using ECS;
using Infrastructure;

namespace Asteroids.ECS.Services
{
    public class EntitySystemService : IEntitySystemService, IRequireCleanUp
    {
        private readonly IUpdateService _updateService;
        private readonly ITimeService _timeService;
        private readonly IEntityWorldService _entityWorldService;

        private readonly ITypeList<IEntitySystem> _systems = new StaticTypeList<IEntitySystem>();

        private bool _isRunning;

        public EntitySystemService(IEntityWorldService entityWorldService, IUpdateService updateService, ITimeService timeService)
        {
            _updateService = updateService;
            _timeService = timeService;
            _entityWorldService = entityWorldService;
        }

        #region IEntitySystemService
        public IEntitySystemService AddSystem<T>() where T : class, IEntitySystem, new()
        {
            return AddSystem(new T());
        }

        public IEntitySystemService AddSystem<T>(T instance) where T : class, IEntitySystem
        {
            instance.World = _entityWorldService.World;
            // This can help in multi-threading decision
            if (instance is IEntitySystemRequireEntityViewWorld requireViewWorld) requireViewWorld.ViewWorld = _entityWorldService.ViewWorld;
            if (instance is IEntitySystemRequireEntityRemover requireRemover) requireRemover.EntityRemover = _entityWorldService.EntityRemover;
            if (instance is IEntitySystemRequireWorldService requireWorldService) requireWorldService.EntityWorldService = _entityWorldService;

            // ToDo: update interfaces too hard -> soft: configure when add
            TryAddSystem<IEntitySystem>(instance);
            TryAddSystem<IEntitySystemRequireFixedUpdate>(instance);
            TryAddSystem<IEntitySystemRequireUpdate>(instance);
            TryAddSystem<IEntitySystemRequireLateUpdate>(instance);

            TryAddSystem<IEntitySystemRequireStart>(instance);
            TryAddSystem<IEntitySystemRequireStop>(instance);

            return this;
        }

        public void RemoveSystem<T>() where T : class, IEntitySystem
        {
            var requiredSystem = _systems.GetFirst<IEntitySystem>(instance => instance is T);
            RemoveSystem(requiredSystem);
        }

        public void RemoveSystem<T>(T instance) where T : class, IEntitySystem
        {
            if (!TryRemoveSystem<IEntitySystem>(instance)) return;

            TryRemoveSystem<IEntitySystemRequireFixedUpdate>(instance);
            TryRemoveSystem<IEntitySystemRequireUpdate>(instance);
            TryRemoveSystem<IEntitySystemRequireLateUpdate>(instance);
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;

            _updateService.FixedUpdate += FixedUpdateHandler;
            _updateService.Update += UpdateHandler;
            _updateService.LateUpdate += LateUpdateHandler;

            var systems = _systems.GetAll<IEntitySystemRequireStart>();
            foreach (var system in systems) system.WorldStarted();
        }

        public void Stop()
        {
            if (!_isRunning) return;
            _isRunning = false;

            _updateService.FixedUpdate -= FixedUpdateHandler;
            _updateService.Update -= UpdateHandler;
            _updateService.LateUpdate -= LateUpdateHandler;

            var systems = _systems.GetAll<IEntitySystemRequireStop>();
            foreach (var system in systems) system.WorldStopped();
        }

        #endregion

        #region Update Handlers
        private void FixedUpdateHandler()
        {
            var systems = _systems.GetAll<IEntitySystemRequireFixedUpdate>();
            foreach (var system in systems)
            {
                system.FixedUpdate(_timeService.FixedDeltaTime);
            }
        }

        private void UpdateHandler()
        {
            var systems = _systems.GetAll<IEntitySystemRequireUpdate>();
            foreach (var system in systems) system.Update(_timeService.DeltaTime);
        }

        private void LateUpdateHandler()
        {
            var systems = _systems.GetAll<IEntitySystemRequireLateUpdate>();
            foreach (var system in systems) system.LateUpdate(_timeService.DeltaTime);
        }
        #endregion

        private bool TryRemoveSystem<T>(IEntitySystem instance) where T : class, IEntitySystem
        {
            if (instance is T requiredInstance) return _systems.Remove(requiredInstance);

            return false;
        }

        private void TryAddSystem<TRequired>(IEntitySystem system) where TRequired : class, IEntitySystem
        {
            if (system is TRequired requiredSystem) _systems.Add(requiredSystem);
        }

        public void CleanUp()
        {
            Stop();
        }
    }
}
