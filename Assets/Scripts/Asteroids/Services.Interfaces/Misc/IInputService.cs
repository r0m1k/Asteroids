using Infrastructure;
using Services;

namespace Asteroids.Services
{
    public interface IInputService : IService, IRequireInitialize
    {
        void Enable();
        void Disable();

        IPlayerShipInput GetPlayerShipInput();
    }
}