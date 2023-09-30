using UnityEngine;

namespace Asteroids.Services
{
    public class AsteroidsRulesService : IAsteroidsRulesService
    {
        private readonly IViewportService _viewportService;
        private readonly IRandomService _randomService;

        public AsteroidsRulesService(IViewportService viewportService, IRandomService randomService)
        {
            _viewportService = viewportService;
            _randomService = randomService;
        }

        public bool InsideViewport(Vector2 point)
        {
            var bounds = _viewportService.GetViewportBounds();
            return bounds.Contains(point);
        }

        public float GetMinLengthToCrossViewport()
        {
            return _viewportService.GetViewportBounds().size.magnitude;
        }

        public Vector2 GetRandomSafePoint(Vector2 respectPoint, float safeRadius)
        {
            var viewportBounds = _viewportService.GetViewportBounds();
            if (safeRadius > viewportBounds.width || safeRadius > viewportBounds.height) return Vector2.zero;

            var sqrSafeRadius = safeRadius * safeRadius;
            while (true)
            {
                var x = _randomService.GetFloat((int) viewportBounds.xMin, (int) viewportBounds.xMax);
                var y = _randomService.GetFloat((int) viewportBounds.yMin, (int) viewportBounds.yMax);
                var spawnPoint = new Vector2(x, y);

                var inPlayerSafeArea = (respectPoint - spawnPoint).sqrMagnitude < sqrSafeRadius;
                if (!inPlayerSafeArea) return spawnPoint;
            }
        }

        public Vector2 GetPointWithRespectToViewportTeleportation(Vector2 point)
        {
            var bounds = _viewportService.GetViewportBounds();
            if (bounds.Contains(point)) return point;

            if (point.x < bounds.xMin) point.x += bounds.width;
            else if (point.x > bounds.xMax) point.x -= bounds.width;

            if (point.y < bounds.yMin) point.y += bounds.height;
            else if (point.y > bounds.yMax) point.y -= bounds.height;

            return point;
        }

        public Vector2 GetFakeClosesPositionWithTeleportationRespect(Vector2 position, Vector2 targetPosition)
        {
            var bounds = _viewportService.GetViewportBounds();
            var fieldSize = bounds.size;
            var fieldHalfSize = fieldSize / 2;

            var dir = targetPosition - position;
            // a) is not fake position but likely fake direction
            // b) is fake position
            var absDirX = Mathf.Abs(dir.x);
            if (absDirX > fieldHalfSize.x)
            {
                dir.x = -Mathf.Sign(dir.x);
                dir.x *= fieldSize.x - absDirX;
            }

            var absDirY = Mathf.Abs(dir.y);
            if (absDirY > fieldHalfSize.y)
            {
                dir.y = -Mathf.Sign(dir.y);
                dir.y *= fieldSize.y - absDirX;
            }

            var fakeClosesPosition = dir + position;
            return fakeClosesPosition;
        }
    }
}