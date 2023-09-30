using Asteroids.Infrastructure;

namespace Asteroids.UIEntityData
{
    public class PlayerWeaponData
    {
        public WeaponType Type;

        public int Bullets;
        public int MaxBullets;

        public float RechargeCooldown;
        public float FireCooldown;
    }
}