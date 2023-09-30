using System.Collections.Generic;
using ECS;

namespace Asteroids.ECS.Components
{
    public class IsCollidedComponent : Component
    {
        public List<IEntity> OtherEntities = new List<IEntity>(3);
    }
}