using Asteroids.Infrastructure;
using Infrastructure;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Services.Generator
{
    public abstract class AsteroidGeometryGeneratorService : IAsteroidGeometryGeneratorService
    {
        protected readonly AsteroidsGeneratorParameters _parameters;
        protected readonly IRandomService _randomService;

        protected AsteroidGeometryGeneratorService(AsteroidsGeneratorParameters parameters, IRandomService randomService)
        {
            _parameters = parameters;
            _randomService = randomService;
        }

        public AsteroidGeometrySpec Generate()
        {
            var asteroid = GenerateSpec(_parameters.Asteroid);
            asteroid.DebrisSpecs = GenerateDebris(in asteroid, _parameters.Debris);

            return asteroid;
        }

        public AsteroidGeometrySpec GenerateSpec(AsteroidGeneratorParameters parameters)
        {
            var baseRadius = _randomService.GetFloat(parameters.Radius);
            var sideCount = _randomService.GetInt(parameters.SideCount);

            var targetAngle = 360f / sideCount;
            var currentAngle = 0f;

            var spec = new AsteroidGeometrySpec();
            spec.Points = new Vector2[sideCount];

            var maxPointRadius = 0f;
            for (var currentSide = 0; currentSide < sideCount; ++currentSide)
            {
                var pointRadius = baseRadius * _randomService.GetFloat(parameters.RadiusModulation);
                var pointAngle = currentAngle + targetAngle * _randomService.GetFloat(parameters.PointDegreeAngleModulation);

                var point = pointRadius * pointAngle.AngleToVector2();
                spec.Points[currentSide] = point;

                maxPointRadius = maxPointRadius.ClampMin(pointRadius);
                currentAngle += targetAngle;
            }

            GenerateMeshUVs(ref spec, maxPointRadius);

            return spec;
        }

        protected abstract void GenerateMeshUVs(ref AsteroidGeometrySpec spec, float maxPointRadius);

        private DebrisGeometrySpec[] GenerateDebris(in AsteroidGeometrySpec asteroidSpec, DebrisGeneratorParameters parameters)
        {
            var asteroidPointsCount = asteroidSpec.Points.Length;

            var debrisCount = _randomService.GetInt(parameters.Count);
            debrisCount = debrisCount.ClampMax(asteroidPointsCount);

            var selectedAsteroidPoints = new List<int>(debrisCount); // not need to use other because count is small
            while (selectedAsteroidPoints.Count < debrisCount)
            {
                var pointIndex = _randomService.GetInt(asteroidPointsCount);
                if (selectedAsteroidPoints.Contains(pointIndex)) continue;

                selectedAsteroidPoints.Add(pointIndex);
            }
            selectedAsteroidPoints.Sort();

            var debrisSpecs = new DebrisGeometrySpec[debrisCount];
            for (var ai = 0; ai < debrisCount; ++ai)
            {
                var spec = new DebrisGeometrySpec();

                var firstPoint = selectedAsteroidPoints[ai];

                var isLastDebris = ai == debrisCount - 1;
                var fakeSecondPoint = isLastDebris ? asteroidPointsCount : selectedAsteroidPoints[ai + 1];

                var pointsCount = fakeSecondPoint - firstPoint + 1;
                spec.Points = new Vector2[pointsCount];
                spec.UVs = new Vector2[pointsCount];

                for (var di = 0; di < pointsCount; ++di)
                {
                    var asteroidPointIndex = firstPoint + di;
                    if (asteroidPointIndex == asteroidPointsCount) asteroidPointIndex = 0;

                    spec.Points[di] = asteroidSpec.Points[asteroidPointIndex];
                    spec.UVs[di] = asteroidSpec.UVs[asteroidPointIndex];
                }

                spec.CenterPoint = CalculateDebrisCenterPoint(in spec);

                debrisSpecs[ai] = spec;
            }

            return debrisSpecs;
        }

        private Vector2 CalculateDebrisCenterPoint(in DebrisGeometrySpec spec)
        {
            var middlePoint = Vector2.zero;
            foreach (var p in spec.Points) middlePoint += p;
            middlePoint /= spec.Points.Length;
            middlePoint /= 2;

            return middlePoint;
        }
    }
}