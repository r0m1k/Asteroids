using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure
{
    public class StaticTypeDictionary<TKey, TItem> : ITypeDictionary<TKey, TItem> where TKey : notnull where TItem : class
    {
        #region Nested
        private static class Container<TContainerItem> where TContainerItem : class, TItem
        {
            private static readonly Dictionary<TKey, TContainerItem> _dictionary = new Dictionary<TKey, TContainerItem>();

            public static void Add(TKey key, TContainerItem instance)
            {
                _dictionary.Add(key, instance);
            }

            public static bool Has(TKey key)
            {
                return _dictionary.ContainsKey(key);
            }

            public static TContainerItem Get(TKey key)
            {
                if (!_dictionary.TryGetValue(key, out var instance)) return default;

                return instance;
            }

            public static bool Remove(TKey key)
            {
                return _dictionary.Remove(key);
            }
        }
        #endregion

        private readonly HashSet<Type> _usedTypes = new HashSet<Type>();

        public T Add<T>(TKey key) where T : class, TItem, new()
        {
            return Add(key, new T());
        }

        public T Add<T>(TKey key, T instance) where T : class, TItem
        {
            Container<T>.Add(key, instance);
            _usedTypes.Add(typeof(T));

            return instance;
        }

        public bool Has<T>(TKey key) where T : class, TItem
        {
            return Container<T>.Has(key);
        }

        public bool Has(TKey key, Type type)
        {
            var containerType = typeof(Container<>).MakeGenericType(typeof(TKey), typeof(TItem), type);
            var method = containerType.GetMethod("Has", BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public);
            var hasComponent = (bool) method.Invoke(null, new object[] { key });

            return hasComponent;
        }

        public T Get<T>(TKey key) where T : class, TItem
        {
            return Container<T>.Get(key);
        }

        public bool Remove<T>(TKey key) where T : class, TItem
        {
            if (!Container<T>.Remove(key)) return false;

            _usedTypes.Remove(typeof(T));

            return true;
        }

        public void RemoveAll(TKey key)
        {
            if (_usedTypes.Count == 0) return;

            var genericContainerType = typeof(Container<>);
            var methodParameters = new object[] { key };
            foreach (var usedComponent in _usedTypes)
            {
                var containerType = genericContainerType.MakeGenericType(usedComponent);
                var method = containerType.GetMethod("Remove", BindingFlags.Static | BindingFlags.InvokeMethod);
                method.Invoke(null, methodParameters);
            }

            _usedTypes.Clear();
        }
    }
}