using UnityEngine;

namespace Asteroids.Services.Generator
{
    public struct AsteroidVisualSpec
    {
        public Mesh Mesh;
        public Vector2[] ColliderPath;

        public DebrisVisualSpec[] Debris;
    }
}