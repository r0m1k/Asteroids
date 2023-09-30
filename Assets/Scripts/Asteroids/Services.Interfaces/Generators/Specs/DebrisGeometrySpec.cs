using UnityEngine;

namespace Asteroids.Services.Generator
{
    public struct DebrisGeometrySpec
    {
        public Vector2 CenterPoint;
        public Vector2[] Points;
        public Vector2[] UVs; // is from -1 to 1
    }
}