using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IReadonlyTypeList<TItem> where TItem : class
    {
        bool Has<T>() where T : class, TItem;
        bool Has(Type type);

        T GetFirst<T>() where T : class, TItem;
        T GetFirst<T>(Predicate<T> predicate) where T : class, TItem;

        T GetFirstOrCreate<T>() where T : class, TItem, new();

        IEnumerable<T> GetAll<T>() where T : class, TItem;
    }

    public interface ITypeList<TItem> : IReadonlyTypeList<TItem> where TItem : class
    {
        T Add<T>() where T : class, TItem, new();
        T Add<T>(T data) where T : class, TItem;

        new T GetFirstOrCreate<T>() where T : class, TItem, new();

        bool Remove<T>(T instance) where T : class, TItem;
        void RemoveAll<T>() where T : class, TItem;
        void RemoveAll();
    }
}