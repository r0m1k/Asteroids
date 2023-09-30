using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class TypeDictionary<TKey, TItem> : ITypeDictionary<TKey, TItem> where TKey : notnull where TItem : class
    {
        private readonly Dictionary<Type, TItem> _dictionary = new Dictionary<Type, TItem>(10);

        public T Add<T>(TKey _) where T : class, TItem, new()
        {
            return Add(_, new T());
        }

        public T Add<T>(TKey _, T instance) where T : class, TItem
        {
            _dictionary.Add(typeof(T), instance);
            return instance;
        }

        public bool Has<T>(TKey _) where T : class, TItem
        {
            return _dictionary.ContainsKey(typeof(T));
        }

        public bool Has(TKey _, Type type)
        {
            return _dictionary.ContainsKey(type);
        }

        public T Get<T>(TKey _) where T : class, TItem
        {
            if (!_dictionary.TryGetValue(typeof(T), out var instance)) return default;

            return instance as T;
        }

        public bool Remove<T>(TKey _) where T : class, TItem
        {
            var instance = Get<T>(_);
            if (instance == null) return false;

            (instance as IRequireCleanUp)?.CleanUp();
            _dictionary.Remove(typeof(T));

            return true;
        }

        public void RemoveAll(TKey _)
        {
            foreach (var value in _dictionary.Values) (value as IRequireCleanUp)?.CleanUp();

            _dictionary.Clear();
        }
    }
}