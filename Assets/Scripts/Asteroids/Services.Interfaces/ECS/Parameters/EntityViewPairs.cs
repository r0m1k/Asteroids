using System.Collections.Generic;

namespace Asteroids.Services.ECS
{
    public struct EntityViewPairs
    {
        public EntityViewPair Main;
        public List<EntityViewPair> Childs;
    }
}