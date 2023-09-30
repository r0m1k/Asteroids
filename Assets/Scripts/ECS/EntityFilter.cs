using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
    public class EntityFilter : IEnumerable<IEntity>
    {
        private readonly IEnumerable<IEntity> _entities;
        private List<Type> _includeTypes;
        private List<Type> _excludeTypes;
        private bool _includeAny = false;
        private bool _excludeAny = true;

        public EntityFilter(IEnumerable<IEntity> entity)
        {
            _entities = entity;
        }

        public EntityFilter Incl<T>() where T : Component
        {
            return IncludeComponent<T>();
        }

        public EntityFilter Incl(params Type[] types)
        {
            return IncludeComponents(types);
        }

        private EntityFilter IncludeComponents(params Type[] types)
        {
            foreach (var type in types) IncludeComponent(type);

            return this;
        }

        private EntityFilter IncludeComponent<T>() where T : Component
        {
            return IncludeComponent(typeof(T));
        }

        private EntityFilter IncludeComponent(Type type)
        {
            AddTypeToCollection(type, ref _includeTypes);

            return this;
        }

        public EntityFilter Excl<T>() where T : Component
        {
            return ExcludeComponent<T>();
        }

        public EntityFilter Excl(params Type[] types)
        {
            return ExcludeComponents(types);
        }

        private EntityFilter ExcludeComponents(params Type[] types)
        {
            foreach (var type in types) ExcludeComponent(type);

            return this;
        }

        private EntityFilter ExcludeComponent<T>() where T : Component
        {
            return ExcludeComponent(typeof(T));
        }

        private EntityFilter ExcludeComponent(Type type)
        {
            AddTypeToCollection(type, ref _excludeTypes);

            return this;
        }

        public EntityFilter InclAll()
        {
            _includeAny = false;
            return this;
        }

        public EntityFilter InclAny()
        {
            _includeAny = true;
            return this;
        }

        public EntityFilter ExclAll()
        {
            _excludeAny = false;
            return this;
        }

        public EntityFilter ExclAny()
        {
            _excludeAny = true;
            return this;
        }

        public IEntity[] GetResult()
        {
            return GetFilter().ToArray();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return GetFilter().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddTypeToCollection(Type type, ref List<Type> collection)
        {
            if (collection == null) collection = new List<Type>(3);

            collection.Add(type);
        }

        private bool IsEntityHasAllComponents(IEntity entity, IReadOnlyList<Type> components)
        {
            foreach (var component in components)
            {
                if (!entity.HasComponent(component)) return false;
            }

            return true;
        }

        private bool IsEntityHasAnyComponents(IEntity entity, IReadOnlyList<Type> components)
        {
            foreach (var component in components)
            {
                if (entity.HasComponent(component)) return true;
            }

            return false;
        }

        private IEnumerable<IEntity> GetFilter()
        {
            Func<IEntity, IReadOnlyList<Type>, bool> includeAction = _includeAny ? IsEntityHasAnyComponents : IsEntityHasAllComponents;
            Func<IEntity, IReadOnlyList<Type>, bool> excludeAction = _excludeAny ? IsEntityHasAnyComponents : IsEntityHasAllComponents;

            var filter = _entities;
            if (_includeTypes?.Count > 0) filter = filter.Where(e => includeAction(e, _includeTypes));
            if (_excludeTypes?.Count > 0) filter = filter.Where(e => !excludeAction(e, _excludeTypes));

            return filter;
        }
    }
}