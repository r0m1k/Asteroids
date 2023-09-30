using UnityEngine;

namespace Asteroids.Services.Generator
{
    public class AsteroidGeometryGeneratorPlainUVsService : AsteroidGeometryGeneratorService
    {
        public AsteroidGeometryGeneratorPlainUVsService(AsteroidsGeneratorParameters parameters, IRandomService randomService) : base(parameters, randomService) { }

        protected override void GenerateMeshUVs(ref AsteroidGeometrySpec spec, float maxPointRadius)
        {
            spec.UVs = new Vector2[spec.Points.Length];
            for (var i = 0; i < spec.Points.Length; ++i)
            {
                var point = spec.Points[i];
                point /= maxPointRadius;
                spec.UVs[i] = point;
            }
        }
    }
}