using Infrastructure;
using System;
using System.Collections;
using UnityEngine;

namespace Asteroids.Services
{
    public class UnityGameObjectService : IUpdateService, ICoroutineService, IRequireCleanUp
    {
        #region Nested
        private class GameObjectHelper : MonoBehaviour
        {
            [NonSerialized]
            public UnityGameObjectService Service;

            private void FixedUpdate()
            {
                Service.FireFixedUpdate();
            }

            private void Update()
            {
                Service.FireUpdate();
            }

            private void LateUpdate()
            {
                Service.FireLateUpdate();
            }
        }
        #endregion

        public event Action FixedUpdate;
        public event Action Update;
        public event Action LateUpdate;

        private readonly GameObjectHelper _helper;

        public UnityGameObjectService()
        {
            var go = new GameObject("UnityGameObjectUpdateServiceHelper");
            _helper = go.AddComponent<GameObjectHelper>();
            _helper.Service = this;
        }

        public Coroutine StartCoroutine(IEnumerator coroutineMethod)
        {
            return _helper.StartCoroutine(coroutineMethod);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _helper.StopCoroutine(coroutine);
        }

        private void FireFixedUpdate()
        {
            FixedUpdate?.Invoke();
        }

        private void FireUpdate()
        {
            Update?.Invoke();
        }

        private void FireLateUpdate()
        {
            LateUpdate?.Invoke();
        }

        public void CleanUp()
        {
            FixedUpdate = null;
            Update = null;
            LateUpdate = null;

            if (_helper)
            {
                _helper.StopAllCoroutines();
                UnityEngine.Object.Destroy(_helper.gameObject);
            }
        }
    }
}