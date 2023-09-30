using System;
using UnityEngine;

namespace Asteroids.Services.Generator
{
    public class AsteroidVisualGeneratorService : IAsteroidVisualGeneratorService
    {
        public AsteroidVisualSpec Generate(AsteroidGeometrySpec geometrySpec)
        {
            var spec = new AsteroidVisualSpec();
            (spec.Mesh, spec.ColliderPath) = GenerateAsteroid(geometrySpec.Points, geometrySpec.UVs);

            spec.Debris = new DebrisVisualSpec[geometrySpec.DebrisSpecs.Length];
            for (var i = 0; i < spec.Debris.Length; ++i)
            {
                (spec.Debris[i].Mesh, spec.Debris[i].ColliderPath) = GenerateDebris(
                    geometrySpec.DebrisSpecs[i].Points,
                    geometrySpec.DebrisSpecs[i].CenterPoint,
                    geometrySpec.DebrisSpecs[i].UVs);

                spec.Debris[i].CenterPoint = geometrySpec.DebrisSpecs[i].CenterPoint;
            }

            return spec;
        }

        private (Mesh, Vector2[]) GenerateAsteroid(Vector2[] points, Vector2[] uvs)
        {
            return GenerateVisual(points, uvs);
        }

        private (Mesh, Vector2[]) GenerateDebris(Vector2[] points, Vector2 center, Vector2[] uvs)
        {
            var centeredPoints = new Vector2[points.Length + 1];
            for (var i = 0; i < points.Length; ++i) centeredPoints[i] = points[i] - center;
            centeredPoints[points.Length] = -center;

            var centeredUVs = new Vector2[uvs.Length + 1];
            Array.ConstrainedCopy(uvs, 0, centeredUVs, 0, uvs.Length);
            centeredUVs[uvs.Length] = new Vector2(0.5f, 0.5f);

            return GenerateVisual(centeredPoints, centeredUVs);
        }

        private (Mesh, Vector2[]) GenerateVisual(Vector2[] points, Vector2[] uvs)
        {
            var mesh = new Mesh();
            var meshVertices = new Vector3[points.Length + 1];
            var meshUV = new Vector2[points.Length + 1];
            var meshTriangles = new int[3 * points.Length];

            var zeroVertexIndex = points.Length;
            meshVertices[zeroVertexIndex] = Vector3.zero;
            meshUV[zeroVertexIndex] = new Vector2(0.5f, 0.5f);

            for (var i = 0; i < points.Length; ++i)
            {
                var isLastPoint = i == points.Length - 1;

                meshVertices[i] = points[i];

                meshUV[i] = NormalizeUV(uvs[i]);

                meshTriangles[3 * i + 0] = zeroVertexIndex;
                meshTriangles[3 * i + 1] = i;
                meshTriangles[3 * i + 2] = isLastPoint ? 0 : i + 1;
            }

            mesh.vertices = meshVertices;
            mesh.uv = meshUV;
            mesh.triangles = meshTriangles;

            var colliderPath = (Vector2[]) points.Clone();

            return (mesh, colliderPath);
        }

        private static Vector2 NormalizeUV(Vector2 v)
        {
            v += Vector2.one;
            v /= 2;
            return v;
        }
    }
}