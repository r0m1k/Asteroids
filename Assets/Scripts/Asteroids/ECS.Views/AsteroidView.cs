using UnityEngine;

namespace Asteroids.ECS.Views
{
    public class AsteroidView : Physics2DView
    {
        public MeshFilter MeshFilter;
        public PolygonCollider2D PolygonCollider;
    }
}