using Asteroids.ECS.Parameters;
using Asteroids.Infrastructure;
using Asteroids.Services.ECS;
using ECS;

namespace Asteroids.ECS.Components
{
    public class WeaponComponent : Component
    {
        public WeaponParameters Parameters;

        public int Bullets;

        public long ShipEntityId;

        public WeaponPriorityType Priority;
    }
}
