using Asteroids.Services;
using Asteroids.UIEntityData;

namespace Asteroids.UI
{
    public abstract class PlayerReadyDataView<T> : DataView<GameData> where T : class, new()
    {
        private IDataReader<T> _otherReader;
        protected IDataReader<T> OtherReader => _otherReader;

        protected override void UpdateState(GameData data)
        {
            if (!data.IsPlayerShipSpawned)
            {
                UpdateState((T)null);
                return;
            }
            Reader.Changed -= DataChangedHandler;

            _otherReader = FindSuitableData();
            if (_otherReader == null) return;

            _otherReader.Changed += OtherDataChanged;
            UpdateState(_otherReader?.Data);
        }

        protected virtual IDataReader<T> FindSuitableData()
        {
            return _dataService.GetFirstOrCreate<T>();
        }

        private void OtherDataChanged(T suitableData)
        {
            UpdateState(suitableData);
        }

        protected abstract void UpdateState(T data);

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_otherReader != null) _otherReader.Changed -= OtherDataChanged;
            _otherReader = null;
        }
    }
}