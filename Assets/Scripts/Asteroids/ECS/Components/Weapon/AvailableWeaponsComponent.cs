using System.Collections.Generic;
using ECS;

namespace Asteroids.ECS.Components
{
    public class AvailableWeaponsComponent : Component
    {
        public List<long> Ids = new List<long>();
    }
}