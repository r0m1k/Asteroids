using UnityEngine;

namespace Asteroids.Services.Generator
{
    [CreateAssetMenu(fileName = "AsteroidsGenerator Parameters", menuName = "Asteroids Game/AsteroidsGenerator Parameters")]
    public class AsteroidsGeneratorParameters : ScriptableObject
    {
        public AsteroidGeneratorParameters Asteroid;
        public DebrisGeneratorParameters Debris;
    }
}