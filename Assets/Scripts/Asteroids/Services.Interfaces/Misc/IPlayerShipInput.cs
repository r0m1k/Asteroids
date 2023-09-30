namespace Asteroids.Services
{
    public interface IPlayerShipInput
    {
        float GetRotation();
        float GetThruster();

        bool IsFirePrimaryWeapon();
        bool IsFireSecondaryWeapon();
    }
}