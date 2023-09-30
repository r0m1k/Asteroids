using ECS;

namespace Asteroids.Services.ECS
{
    public struct EntityViewPair
    {
        public IEntity Entity;
        public EntityView View;
    }
}