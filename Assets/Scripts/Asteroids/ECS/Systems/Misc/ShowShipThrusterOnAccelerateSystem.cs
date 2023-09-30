using Asteroids.ECS.Components;
using Asteroids.ECS.Views;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class ShowShipThrusterOnAccelerateSystem : ViewEntitySystem, IEntitySystemRequireUpdate
    {
        public void Update(float deltaTime)
        {
            var entities = World.FilterByComponents<IsHasThrusterComponent, MovementComponent>();
            foreach (var entity in entities)
            {
                var movement = entity.GetComponent<MovementComponent>();
                var view = ViewWorld.Get(entity.Id);

                var thrusterView = view as IHasThrusterView;
                if (thrusterView?.ThrusterEffect == null) continue;

                thrusterView.ThrusterEffect.SetActive(movement.IsAccelerating);
            }
        }
    }
}