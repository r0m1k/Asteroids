using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class TypeList<TItem> : ITypeList<TItem> where TItem : class
    {
        private abstract class Container : IRequireCleanUp
        {
            public abstract bool HasValues();
            public abstract void RemoveAll();
            public abstract void CleanUp();
        }

        private class Container<TContainerItem> : Container where TContainerItem : class, TItem
        {
            private readonly List<TContainerItem> _list = new List<TContainerItem>();

            public int Count => _list.Count;
            
            public void Add(TContainerItem item)
            {
                _list.Add(item);
            }

            public override bool HasValues()
            {
                return _list.Count > 0;
            }

            public TContainerItem GetFirst()
            {
                return _list.Count > 0 ? _list[0] : default;
            }

            public TContainerItem GetFirst(Predicate<TContainerItem> predicate)
            {
                return _list.Find(predicate);
            }

            public IEnumerator<TContainerItem> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public bool Remove(TContainerItem item)
            {
                var removed = _list.Remove(item);
                (item as IRequireCleanUp)?.CleanUp();

                return removed;
            }

            public override void RemoveAll()
            {
                if (typeof(IRequireCleanUp).IsAssignableFrom(typeof(TContainerItem)))

                    foreach (var item in _list) (item as IRequireCleanUp)?.CleanUp();
                _list.Clear();
            }

            public override void CleanUp()
            {
                RemoveAll();
            }
        }

        private readonly Dictionary<Type, Container> _data = new Dictionary<Type, Container>(50);

        #region IReadonlyTypeList

        public bool Has<T>() where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);
            return container.HasValues();
        }

        public bool Has(Type type)
        {
            var container = GetDataContainerList(type, false);
            return container?.HasValues() ?? false;
        }

        public T GetFirst<T>() where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);
            return container?.GetFirst();
        }

        public T GetFirst<T>(Predicate<T> predicate) where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);
            return container.GetFirst(predicate);
        }

        T IReadonlyTypeList<TItem>.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        public IEnumerable<T> GetAll<T>() where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);

            if (!container.HasValues()) return new List<T>();

            var retList = new List<T>(container.Count);
            foreach (var item in container) retList.Add(item as T);

            return retList;
        }

        #endregion

        #region ITypeList

        public T Add<T>() where T : class, TItem, new()
        {
            return Add(new T());
        }

        public T Add<T>(T data) where T : class, TItem
        {
            var container = GetDataContainerList<T>(true);
            container.Add(data);

            return data;
        }

        T ITypeList<TItem>.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        public bool Remove<T>(T instance) where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);
            return container.Remove(instance);
        }

        public void RemoveAll<T>() where T : class, TItem
        {
            var container = GetDataContainerList<T>(false);
            container?.RemoveAll();
        }

        public void RemoveAll()
        {
            foreach (var typeListPair in _data) typeListPair.Value.RemoveAll();
        }

        #endregion

        private T GetFirstOrCreate<T>() where T : class, TItem, new()
        {
            var data = GetFirst<T>();
            if (data == null) data = Add<T>();

            return data;
        }

        private Container<T> GetDataContainerList<T>(bool createIfNotExists) where T : class, TItem
        {
            return GetDataContainerList(typeof(T), createIfNotExists) as Container<T>;
        }

        private Container GetDataContainerList(Type type, bool createIfNotExists)
        {
            _data.TryGetValue(type, out var container);
            if (container == null && createIfNotExists) container = _data[type] = Activator.CreateInstance(typeof(Container<>).MakeGenericType(typeof(TItem), type)) as Container;

            return container;
        }
    }
}