using UnityEngine;

namespace Asteroids.Services.Generator
{
    public class AsteroidGeometryGeneratorRoundedUVsService : AsteroidGeometryGeneratorService
    {
        public AsteroidGeometryGeneratorRoundedUVsService(AsteroidsGeneratorParameters parameters, IRandomService randomService) : base(parameters, randomService) { }

        protected override void GenerateMeshUVs(ref AsteroidGeometrySpec spec, float maxPointRadius)
        {
            spec.UVs = new Vector2[spec.Points.Length];
            for (var i = 0; i < spec.Points.Length; ++i)
            {
                var point = spec.Points[i];

                var minValue = Mathf.Min(Mathf.Abs(point.x), Mathf.Abs(point.y));
                var maxValue = Mathf.Max(Mathf.Abs(point.x), Mathf.Abs(point.y));
                var targetLength = maxPointRadius * Mathf.Sqrt(1 + minValue * minValue / (maxValue * maxValue));

                var pointOnQuad = targetLength * point.normalized;
                pointOnQuad /= maxPointRadius;

                spec.UVs[i] = pointOnQuad;
            }
        }
    }
}