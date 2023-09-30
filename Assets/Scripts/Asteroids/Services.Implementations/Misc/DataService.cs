using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Services
{
    public abstract class DataService : IDataService
    {
        #region Nested

        protected abstract class DataContainer
        {
            public abstract void Clear();
        }

        protected class DataContainer<T> : DataContainer, IDataWriter<T> where T : class
        {
            public T Data { get; }

            public event Action<T> Changed;

            public DataContainer(T data)
            {
                Data = data;
            }

            public void Notify()
            {
                Changed?.Invoke(Data);
            }

            public override void Clear()
            {
                Changed = null;
            }
        }

        #endregion

        private readonly ITypeList<DataContainer> _container;
        private readonly DataServiceParameters _parameters;

        protected DataService(ITypeList<DataContainer> container, DataServiceParameters parameters)
        {
            _container = container;
            _parameters = parameters;
        }

        #region IReadOnly DataService interface

        IDataReader<T> IReadOnlyDataService.GetFirst<T>()
        {
            return GetFirst<T>();
        }

        IDataReader<T> IReadOnlyDataService.GetFirst<T>(Predicate<T> predicate)
        {
            return _container.GetFirst<DataContainer<T>>(data => predicate(data.Data));
        }

        IDataReader<T> IReadOnlyDataService.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        IEnumerable<IDataReader<T>> IReadOnlyDataService.GetAll<T>()
        {
            return _container.GetAll<DataContainer<T>>();
        }

        T IReadOnlyDataService.GetScriptable<T>()
        {
            if (_parameters == null) return default;

            var ret = _parameters.Scriptables.FirstOrDefault(s => s is T) as T;

            return ret;
        }

        #endregion

        #region IDataService interface

        IDataWriter<T> IDataService.Add<T>()
        {
            return (this as IDataService).Add(new T());
        }

        IDataWriter<T> IDataService.Add<T>(T data)
        {
            var dataContainer = new DataContainer<T>(data);

            _container.Add(dataContainer);

            return dataContainer;
        }

        IDataWriter<T> IDataService.GetFirst<T>()
        {
            return GetFirst<T>();
        }

        IDataWriter<T> IDataService.GetFirstOrCreate<T>()
        {
            return GetFirstOrCreate<T>();
        }

        void IDataService.RemoveAll<T>()
        {
            _container.RemoveAll<DataContainer<T>>();
        }

        void IDataService.RemoveAll()
        {
            _container.RemoveAll();
        }

        void IRequireCleanUp.CleanUp()
        {
            //foreach (var typeListPair in _data) typeListPair.Value.Clear();
            //_data.Clear();
        }

        #endregion

        private IDataWriter<T> GetFirst<T>() where T : class
        {
            return _container.GetFirst<DataContainer<T>>();
        }

        private DataContainer<T> GetFirstOrCreate<T>() where T : class, new()
        {
            var item = _container.GetFirst<DataContainer<T>>();
            if (item == null) item = _container.Add(new DataContainer<T>(new T()));

            return item;
        }
    }
}