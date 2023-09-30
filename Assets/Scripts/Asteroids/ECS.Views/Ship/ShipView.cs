using UnityEngine;

namespace Asteroids.ECS.Views
{
    public class ShipView : Physics2DView, IHasThrusterView
    {
        public Transform WeaponPivot;
        public GameObject ThrusterEffect;

        GameObject IHasThrusterView.ThrusterEffect => ThrusterEffect;
    }
}