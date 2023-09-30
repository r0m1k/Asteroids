using UnityEngine;

namespace Asteroids.Services.Generator
{
    public struct DebrisVisualSpec
    {
        public Mesh Mesh;
        public Vector2[] ColliderPath;

        public Vector2 CenterPoint;
    }
}