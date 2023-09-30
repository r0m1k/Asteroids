using System;
using Infrastructure;

namespace Asteroids.Services.Generator
{
    [Serializable]
    public class DebrisGeneratorParameters
    {
        [MinMaxRange(2, 10)]
        public MinMaxIntRange Count;
    }
}