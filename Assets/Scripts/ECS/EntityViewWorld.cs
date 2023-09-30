using System.Collections.Generic;

namespace ECS
{
    public class EntityViewWorld : IEntityViewWorld
    {
        private readonly Dictionary<long, IEntityView> _entities = new Dictionary<long, IEntityView>(50);

        public TEntityView Get<TEntityView>(long id) where TEntityView : class, IEntityView
        {
            _entities.TryGetValue(id, out var view);

            return view as TEntityView;
        }

        public IEntityView Get(long id)
        {
            _entities.TryGetValue(id, out var view);

            return view;
        }

        public void Remove(long id)
        {
            _entities.Remove(id);
        }

        public void Add(IEntityView view)
        {
            _entities.Add(view.EntityId, view);
        }
    }
}