using Services;
using UnityEngine;

namespace Asteroids.Services
{
    public interface IAssetService : IService
    {
        T Load<T>(string path) where T : UnityEngine.Object;

        GameObject Instantiate(GameObject prefab);
        GameObject Instantiate(GameObject prefab, Transform parent);
        T Instantiate<T>(T prefab) where T : MonoBehaviour;
        T Instantiate<T>(T prefab, Transform parent) where T : MonoBehaviour;

        void Destroy(GameObject instance);

        void RequestFreeUnused();
    }
}