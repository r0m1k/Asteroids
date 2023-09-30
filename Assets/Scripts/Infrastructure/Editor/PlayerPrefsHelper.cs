using UnityEditor;
using UnityEngine;

namespace Infrastructure
{
    public static class PlayerPrefsHelper
    {
        [MenuItem("Tools/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}