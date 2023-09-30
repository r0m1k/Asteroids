using Asteroids.Services;

namespace Asteroids.UI
{
    public abstract class DataView<T> : View where T : class, new()
    {
        private IDataReader<T> _reader;
        protected IDataReader<T> Reader => _reader;

        protected override void InitializeInternal()
        {
            _reader = GetReader();
            _reader.Changed += DataChangedHandler;

            UpdateState(_reader.Data);
        }

        protected virtual IDataReader<T> GetReader()
        {
            return _dataService.GetFirstOrCreate<T>();
        }

        protected virtual void OnDestroy()
        {
            if (_reader != null) _reader.Changed -= DataChangedHandler;
            _reader = null;
        }

        protected void DataChangedHandler(T data)
        {
            UpdateState(data);
        }

        protected abstract void UpdateState(T data);
    }
}