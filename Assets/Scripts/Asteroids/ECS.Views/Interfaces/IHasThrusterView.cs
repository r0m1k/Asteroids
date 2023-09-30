using UnityEngine;

namespace Asteroids.ECS.Views
{
    public interface IHasThrusterView
    {
        public GameObject ThrusterEffect { get; }
    }
}