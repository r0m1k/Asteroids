using Infrastructure;
using Services;

namespace Asteroids.Services
{
    public interface ILoadScreenService : IService, IRequireInitialize
    {
        void ShowImmediately();
        void HideImmediately();
    }
}