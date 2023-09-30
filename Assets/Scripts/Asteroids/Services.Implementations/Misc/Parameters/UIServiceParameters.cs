using Asteroids.UI;
using UnityEngine;

namespace Asteroids.Services
{
    [CreateAssetMenu(fileName = "UIService Parameters", menuName = "Asteroids Infrastructure/UIService Parameters")]
    public class UIServiceParameters : ScriptableObject
    {
        public RootView RootPrefab;
        public View[] HudElementsPrefabs;

        public PlayerDeathDialogView PlayerDeathDialogPrefab;
    }
}