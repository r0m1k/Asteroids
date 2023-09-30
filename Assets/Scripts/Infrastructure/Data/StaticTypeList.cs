using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure
{
    public class StaticTypeList<TItem> : ITypeList<TItem>, IRequireCleanUp where TItem : class
    {
        #region Nested
        private static class Container<TContainerItem> where TContainerItem : class, TItem
        {
            private static readonly List<TContainerItem> _list = new List<TContainerItem>();

            public static bool HasItems() => _list.Count > 0;

            public static void Add(TContainerItem item)
            {
                _list.Add(item);
            }

            public static TContainerItem GetFirst()
            {
                return _list.Count > 0 ? _list[0] : default;
            }

            public static TContainerItem GetFirst(Predicate<TContainerItem> predicate)
            {
                return _list.Find(predicate);
            }

            public static IEnumerable<TContainerItem> GetEnumerable()
            {
                return _list;
            }

            public static bool Remove(TContainerItem item)
            {
                var removed = _list.Remove(item);
                (item as IRequireCleanUp)?.CleanUp();

                return removed;
            }

            public static void RemoveAll()
            {
                if (typeof(IRequireCleanUp).IsAssignableFrom(typeof(TContainerItem)))
                {
                    foreach (var item in _list) (item as IRequireCleanUp).CleanUp();
                }

                _list.Clear();
            }
        }
        #endregion

        private readonly HashSet<Type> _usedTypes = new HashSet<Type>();

        #region IReadonlyTypeList

        public bool Has<T>() where T : class, TItem
        {
            return Container<T>.HasItems();
        }

        public bool Has(Type type)
        {
            var containerType = typeof(Container<>).MakeGenericType(type);
            var method = containerType.GetMethod("HasItems", BindingFlags.Static | BindingFlags.InvokeMethod);
            var hasItems = (bool) method.Invoke(null, null);

            return hasItems;
        }

        public T GetFirst<T>() where T : class, TItem
        {
            return Container<T>.GetFirst();
        }

        public T GetFirst<T>(Predicate<T> predicate) where T : class, TItem
        {
            return Container<T>.GetFirst(predicate);
        }

        T IReadonlyTypeList<TItem>.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        public IEnumerable<T> GetAll<T>() where T : class, TItem
        {
            return Container<T>.GetEnumerable();
        }

        #endregion

        #region ITypeList

        public T Add<T>() where T : class, TItem, new()
        {
            return Add(new T());
        }

        public T Add<T>(T instance) where T : class, TItem
        {
            Container<T>.Add(instance);
            _usedTypes.Add(typeof(T));

            return instance;
        }

        T ITypeList<TItem>.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        public bool Remove<T>(T instance) where T : class, TItem
        {
            var removed = Container<T>.Remove(instance);

            if (!Container<T>.HasItems()) _usedTypes.Remove(typeof(T));

            return removed;
        }

        public void RemoveAll<T>() where T : class, TItem
        {
            Container<T>.RemoveAll();
            _usedTypes.Remove(typeof(T));
        }

        public void RemoveAll()
        {
            if (_usedTypes.Count == 0) return;

            var genericContainerType = typeof(Container<>);
            foreach (var usedComponent in _usedTypes)
            {
                var containerType = genericContainerType.MakeGenericType(usedComponent);
                var method = containerType.GetMethod("RemoveAll", BindingFlags.Static | BindingFlags.InvokeMethod);
                method.Invoke(null, null);
            }

            _usedTypes.Clear();
        }
        #endregion

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        private T GetFirstOrCreate<T>() where T : class, TItem, new()
        {
            var instance = GetFirst<T>();
            if (instance == null) instance = Add<T>();

            return instance;
        }
    }
}