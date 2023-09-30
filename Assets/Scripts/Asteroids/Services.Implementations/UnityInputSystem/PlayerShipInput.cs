namespace Asteroids.Services.UnityInputSystem
{
    internal class PlayerShipInput : IPlayerShipInput
    {
        private readonly AsteroidInputs.PlayerActions _actions;

        public PlayerShipInput(AsteroidInputs.PlayerActions actions)
        {
            _actions = actions;
        }

        public float GetRotation()
        {
            return _actions.Rotation.ReadValue<float>();
        }

        public float GetThruster()
        {
            return _actions.Thruster.ReadValue<float>();
        }

        public bool IsFirePrimaryWeapon()
        {
            return _actions.FirePrimaryWeapon.IsPressed();
        }

        public bool IsFireSecondaryWeapon()
        {
            return _actions.FireSecondaryWeapon.IsPressed();
        }
    }
}