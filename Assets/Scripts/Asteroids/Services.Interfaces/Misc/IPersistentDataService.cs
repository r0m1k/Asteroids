using Infrastructure;
using Services;

namespace Asteroids.Services
{
    public interface IPersistentDataService : IService, IRequireCleanUp
    {
        void Set(string key, bool value);
        void Set(string key, string value);
        void Set<T>(string key, T instance);

        bool GetBool(string key);
        string GetString(string key);
        T GetAs<T>(string key);
    }
}