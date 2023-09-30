using UnityEngine;

namespace Asteroids.Services
{
    [CreateAssetMenu(fileName = "LoadScreen Parameters", menuName = "Asteroids Infrastructure/LoadScreen Parameters")]
    public class LoadScreenServiceParameters : ScriptableObject
    {
        public GameObject Prefab;
    }
}