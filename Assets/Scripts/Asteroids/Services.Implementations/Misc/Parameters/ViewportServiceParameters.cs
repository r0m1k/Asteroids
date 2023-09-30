using UnityEngine;

namespace Asteroids.Services
{
    [CreateAssetMenu(fileName = "ViewportService Parameters", menuName = "Asteroids Game/ViewportService Parameters")]
    public class ViewportServiceParameters : ScriptableObject
    {
        public GameObject Prefab;

        [Range(320, 3000)]
        public int VerticalSpacefieldSize;
    }
}