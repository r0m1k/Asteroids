using UnityEngine;

namespace Asteroids.Services
{
    // don't bother with Json servialize/deserialize service...
    public class PersistentDataService : IPersistentDataService
    {
        public void CleanUp()
        {
            PlayerPrefs.Save();
        }

        public void Set(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void Set<T>(string key, T instance)
        {
            var json = JsonUtility.ToJson(instance);
            Set(key, json);
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) != 0;
        }

        public T GetAs<T>(string key)
        {
            var json = GetString(key);
            return JsonUtility.FromJson<T>(json);
        }
    }
}