using UnityEngine;

namespace Asteroids.Services
{
    public class AssetService : IAssetService
    {
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(GameObject prefab)
        {
            return Instantiate(prefab, null);
        }

        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }

        public T Instantiate<T>(T prefab) where T : MonoBehaviour
        {
            return Instantiate<T>(prefab, null);
        }

        public T Instantiate<T>(T prefab, Transform parent) where T : MonoBehaviour
        {
            var go = Instantiate(prefab.gameObject, parent);
            return go.GetComponent<T>();
        }

        public void Destroy(GameObject instance)
        {
            Object.Destroy(instance);
        }

        public void RequestFreeUnused()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}