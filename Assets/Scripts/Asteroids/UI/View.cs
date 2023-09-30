using Asteroids.Services;
using UnityEngine;

namespace Asteroids.UI
{
    public class View : MonoBehaviour, IUIView
    {
        protected IUIViewRoot _uiRoot;
        protected IReadOnlyDataService _dataService;

        public void Initialize(IUIViewRoot uiRoot, IReadOnlyDataService dataService)
        {
            _uiRoot = uiRoot;
            _dataService = dataService;

            InitializeInternal();
        }

        protected virtual void InitializeInternal() { }
    }
}