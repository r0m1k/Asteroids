using Asteroids.ECS.Parameters;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Components
{
    public class BulletComponent : Component
    {
        public WeaponParameters Parameters;

        public long WeaponEntityId;
    }
}