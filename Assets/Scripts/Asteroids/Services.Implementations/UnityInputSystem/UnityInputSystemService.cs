using Infrastructure;
using UnityEngine;

namespace Asteroids.Services.UnityInputSystem
{
    public class UnityInputSystemService : IInputService, IRequireCleanUp
    {
        private GameObject _inputInstance;

        private AsteroidInputs _asteroidInputs;
        private PlayerShipInput _playerShipInput;

        public virtual void Initialize()
        {
            _asteroidInputs = new AsteroidInputs();
            _playerShipInput = new PlayerShipInput(_asteroidInputs.Player);
        }

        public void Enable()
        {
            _asteroidInputs.Enable();
        }

        public void Disable()
        {
            _asteroidInputs.Disable();
        }

        public IPlayerShipInput GetPlayerShipInput()
        {
            return _playerShipInput;
        }

        public void CleanUp()
        {
            _asteroidInputs?.Dispose();
            _asteroidInputs = null;
            _playerShipInput = null;
        }
    }
}