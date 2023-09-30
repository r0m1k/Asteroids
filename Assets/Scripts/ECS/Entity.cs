using System;
using Infrastructure;

namespace ECS
{
    public class Entity : IEntity, IEntitySetId
    {
        public long Id { get; private set; }

        // one the TComponent instance per entity
        private readonly ITypeDictionary<long, Component> _componentContainer;

        public Entity()
        {
            _componentContainer = new TypeDictionary<long, Component>();
            //_componentContainer = new StaticTypeDictionary<long, Component>();
        }

        public void SetId(long id)
        {
            Id = id;

            AddComponents();
        }

        protected virtual void AddComponents() { }

        public T AddComponent<T>() where T : Component, new()
        {
            return AddComponent(new T());
        }

        public T AddComponent<T>(T instance) where T : Component
        {
            return _componentContainer.Add(Id, instance);
        }

        public T AddComponentIfNotExists<T>() where T : Component, new()
        {
            var component = GetComponent<T>();
            if (component == null) component = AddComponent<T>();

            return component;
        }

        public bool HasComponent<T>() where T : Component
        {
            return _componentContainer.Has<T>(Id);
        }

        public bool HasComponent(Type type)
        {
            return _componentContainer.Has(Id, type);
        }

        public T GetComponent<T>() where T : Component
        {
            return _componentContainer.Get<T>(Id);
        }

        public T GetOrCreateComponent<T>() where T : Component, new()
        {
            var component = GetComponent<T>();
            if (component == null) component = AddComponent<T>();

            return component;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            return _componentContainer.Remove<T>(Id);
        }

        public void RemoveAllComponents()
        {
            _componentContainer.RemoveAll(Id);
        }
    }
}