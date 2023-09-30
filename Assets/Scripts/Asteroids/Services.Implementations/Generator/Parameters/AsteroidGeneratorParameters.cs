using System;
using Infrastructure;

namespace Asteroids.Services.Generator
{
    [Serializable]
    public class AsteroidGeneratorParameters
    {
        [MinMaxRange(5, 200)]
        public MinMaxIntRange Radius;

        [MinMaxRange(0.5f, 2f)]
        public MinMaxFloatRange RadiusModulation;

        [MinMaxRange(3, 15)]
        public MinMaxIntRange SideCount;

        [MinMaxRange(-0.4f, 0.4f)]
        public MinMaxFloatRange PointDegreeAngleModulation;
    }
}