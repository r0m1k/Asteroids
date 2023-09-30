using System;

namespace Infrastructure
{
    public interface IReadOnlyTypeDictionary<TKey, TItem> where TItem : class
    {
        bool Has<T>(TKey key) where T : class, TItem;
        bool Has(TKey key, Type type);

        T Get<T>(TKey key) where T : class, TItem;
    }

    public interface ITypeDictionary<TKey, TItem> : IReadOnlyTypeDictionary<TKey, TItem> where TItem : class
    {
        T Add<T>(TKey key) where T : class, TItem, new();
        T Add<T>(TKey key, T instance) where T : class, TItem;

        bool Remove<T>(TKey key) where T : class, TItem;
        void RemoveAll(TKey key);
    }
}