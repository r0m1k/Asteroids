using UnityEngine;

namespace Asteroids.Services.Generator
{
    public struct AsteroidGeometrySpec
    {
        public Vector2[] Points;
        public Vector2[] UVs; // is from -1 to 1

        public DebrisGeometrySpec[] DebrisSpecs;
    }
}