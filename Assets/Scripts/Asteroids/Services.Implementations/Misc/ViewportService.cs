using Infrastructure;
using UnityEngine;

namespace Asteroids.Services
{
    public class ViewportService : IViewportService, IRequireCleanUp
    {
        private readonly IAssetService _assetService;
        private readonly ViewportServiceParameters _parameters;

        private GameObject _viewportInstance;
        private ViewportView _viewportView;

        public ViewportService(ViewportServiceParameters parameters, IAssetService assetService)
        {
            _assetService = assetService;
            _parameters = parameters;
        }

        public Rect GetViewportBounds()
        {
            var height = 2 * _viewportView.Camera.orthographicSize;
            var width = _viewportView.Camera.aspect * height;

            return new Rect(-width / 2, -height / 2, width, height);
        }

        public void Initialize()
        {
            _viewportInstance = _assetService.Instantiate(_parameters.Prefab);
            _viewportView = _viewportInstance.GetComponent<ViewportView>();
            _viewportView.Camera.orthographicSize = _parameters.VerticalSpacefieldSize / 2f;
        }

        public void Update() { }

        public void CleanUp()
        {
            if (_viewportInstance != null) _assetService.Destroy(_viewportInstance);
            _viewportInstance = null;
            _viewportView = null;
        }
    }
}