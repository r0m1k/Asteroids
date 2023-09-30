using Asteroids.ECS.Components;
using ECS;

namespace Asteroids.ECS.Entities
{
    public class WeaponEntity : Entity
    {
        protected override void AddComponents()
        {
            base.AddComponents();

            AddComponent<WeaponComponent>();
        }
    }
}