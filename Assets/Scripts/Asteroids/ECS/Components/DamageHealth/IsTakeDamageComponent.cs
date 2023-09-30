using ECS;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.ECS.Components
{
    public class IsTakeDamageComponent : Component
    {
        public List<int> Values = new List<int>(1);
        public int Value => Values.Sum();
    }
}
