using Infrastructure;
using UnityEngine;

namespace Asteroids.Services
{
    public class LoadScreenService : ILoadScreenService, IRequireCleanUp
    {
        protected IAssetService _assetService;
        protected LoadScreenServiceParameters _parameters;

        protected GameObject _uiInstance;

        public LoadScreenService(LoadScreenServiceParameters parameters, IAssetService assetService)
        {
            _assetService = assetService;
            _parameters = parameters;
        }

        public virtual void Initialize()
        {
            _uiInstance = _assetService.Instantiate(_parameters.Prefab);
        }

        public virtual void ShowImmediately()
        {
            _uiInstance.SetActive(true);
        }

        public virtual void HideImmediately()
        {
            _uiInstance.SetActive(false);
        }

        public void CleanUp()
        {
            if (_uiInstance != null) _assetService.Destroy(_uiInstance);
            _uiInstance = null;
        }
    }
}