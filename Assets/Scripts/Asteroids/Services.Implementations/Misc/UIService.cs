using Asteroids.UI;
using System;
using System.Collections.Generic;

namespace Asteroids.Services
{
    public class UIService : IUIService, IUIViewRoot
    {
        private readonly UIServiceParameters _parameters;
        private readonly IAssetService _assetService;
        private readonly IDataService _dataService;

        private RootView _root;

        private readonly List<View> _persistentViews = new List<View>(5);
        private readonly Stack<View> _dynamicViews = new Stack<View>(5);

        public UIService(UIServiceParameters parameters, IAssetService assetService, IDataService dataService)
        {
            _assetService = assetService;
            _parameters = parameters;
            _dataService = dataService;
        }

        #region Interfaces methods

        public void Initialize()
        {
            _root = _assetService.Instantiate(_parameters.RootPrefab);
        }

        public void CreateHud()
        {
            foreach (var hudElementPrefab in _parameters.HudElementsPrefabs)
            {
                var view = CreateView(hudElementPrefab);
                AddPersistent(view);
            }
        }

        public void ShowHud()
        {
            foreach (var view in _persistentViews) view.gameObject.SetActive(true);
        }

        public void HideHud()
        {
            foreach (var view in _persistentViews) view.gameObject.SetActive(false);
        }

        public void ShowPlayerDeathDialog(Action restartCallback)
        {
            var dialog = CreateView(_parameters.PlayerDeathDialogPrefab);
            dialog.RestartCallback = restartCallback;

            PushDynamicView(dialog);
        }

        public void PushView(View view)
        {
            PushDynamicView(view);
        }

        public void PopView()
        {
            PopDynamicView();
        }

        void IUIService.PopAllViews()
        {
            PopAllViews();
        }

        void IUIViewRoot.PopAllViews()
        {
            PopAllViews();
        }

        public void CleanUp()
        {
            PopAllViews();
            RemoveAllPersistents();

            if (_root) _assetService.Destroy(_root.gameObject);
        }

        #endregion

        private T CreateView<T>(T prefab) where T : View
        {
            var view = _assetService.Instantiate(prefab, _root.ViewsRoot);
            view.Initialize(this, _dataService);

            return view;
        }

        private void AddPersistent(View view)
        {
            _persistentViews.Add(view);
        }

        private void RemovePersistent(View view)
        {
            if (!_persistentViews.Remove(view)) return;

            DestroyView(view);
        }

        private void RemoveAllPersistents()
        {
            while (_persistentViews.Count > 0) RemovePersistent(_persistentViews[0]);
        }

        private void PushDynamicView(View view)
        {
            _dynamicViews.Push(view);
        }

        private void PopDynamicView()
        {
            if (_dynamicViews.Count == 0) return;

            var view = _dynamicViews.Pop();
            DestroyView(view);
        }

        void PopAllViews()
        {
            while (_dynamicViews.Count > 0) PopDynamicView();
        }

        private void DestroyView(View view)
        {
            if (view == null) return;

            _assetService.Destroy(view.gameObject);
        }
    }
}