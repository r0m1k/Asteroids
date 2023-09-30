using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
    public class EntityWorld : IEntityWorld
    {
        private readonly Dictionary<long, IEntity> _entities = new Dictionary<long, IEntity>(50);

        public IEntity Get(long id)
        {
            _entities.TryGetValue(id, out var entity);
            return entity;
        }

        public T Get<T>(long id) where T : class, IEntity
        {
            _entities.TryGetValue(id, out var entity);

            return entity as T;
        }

        public IEntity[] GetAll()
        {
            return _entities
                .Values
                .ToArray();
        }

        public IEntity[] GetAll(Predicate<IEntity> predicate)
        {
            return _entities
                .Values
                .Where(e => predicate(e))
                .ToArray();
        }

        public T FindFirst<T>() where T : class, IEntity
        {
            return _entities.Values.FirstOrDefault(e => e is T) as T;
        }

        public T[] Find<T>() where T : class, IEntity
        {
            return _entities.Values.OfType<T>().ToArray();
        }

        public bool Visit<T>(Action<IEntity> visitor) where T : class, IEntity
        {
            var visited = false;
            foreach (var entity in _entities.Values)
            {
                if (entity is T typedEntity)
                {
                    visited = true;
                    visitor(typedEntity);
                }
            }

            return visited;
        }


        public EntityFilter CreateFilter()
        {
            return new EntityFilter(_entities.Values);

        }
        public EntityFilter FilterByComponents<T1>() where T1 : Component
        {
            return CreateFilter()
                .Incl<T1>();
        }

        public EntityFilter FilterByComponents<T1, T2>() where T1 : Component where T2 : Component
        {
            return CreateFilter()
                .Incl<T1>()
                .Incl<T2>();
        }

        public EntityFilter FilterByComponents<T1, T2, T3>() where T1 : Component where T2 : Component where T3 : Component
        {
            return CreateFilter()
                .Incl<T1>()
                .Incl<T2>()
                .Incl<T3>();
        }

        public EntityFilter FilterByComponents<T1, T2, T3, T4>() where T1 : Component where T2 : Component where T3 : Component where T4 : Component
        {
            return CreateFilter()
                .Incl<T1>()
                .Incl<T2>()
                .Incl<T3>()
                .Incl<T4>();
        }

        public EntityFilter FilterByComponents(params Type[] componentsTypes)
        {
            return CreateFilter()
                .Incl(componentsTypes);
        }

        public long Add(IEntity entity)
        {
            _entities.Add(entity.Id, entity);

            return entity.Id;
        }

        public bool Remove(long id)
        {
            if (!_entities.ContainsKey(id)) return false;

            _entities.Remove(id);

            return true;
        }
    }
}