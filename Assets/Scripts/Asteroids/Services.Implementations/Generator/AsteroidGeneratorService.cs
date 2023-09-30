namespace Asteroids.Services.Generator
{
    public class AsteroidGeneratorService : IAsteroidGeneratorService
    {
        private readonly AsteroidsGeneratorParameters _parameters;
        private readonly IRandomService _randomService;
        private readonly IAsteroidGeometryGeneratorService _geometryGeneratorService;
        private readonly IAsteroidVisualGeneratorService _visualGeneratorService;

        public AsteroidGeneratorService(AsteroidsGeneratorParameters parameters,
            IRandomService randomService,
            IAsteroidGeometryGeneratorService geometryGeneratorService,
            IAsteroidVisualGeneratorService visualGeneratorService)
        {
            _parameters = parameters;
            _randomService = randomService;
            _geometryGeneratorService = geometryGeneratorService;
            _visualGeneratorService = visualGeneratorService;
        }

        public AsteroidSpec Generate()
        {
            var geometry = _geometryGeneratorService.Generate();
            var visual = _visualGeneratorService.Generate(geometry);
            var asteroid = Generate(visual);

            return asteroid;
        }

        public AsteroidSpec Generate(AsteroidVisualSpec visualSpec)
        {
            var asteroidSpec = new AsteroidSpec();

            asteroidSpec.VisualSpec = visualSpec;

            GenerateAsteroid(ref asteroidSpec);
            asteroidSpec.DebrisSpecs = GenerateDebris(in asteroidSpec, visualSpec.Debris.Length);

            return asteroidSpec;
        }

        protected void GenerateAsteroid(ref AsteroidSpec spec)
        {
            var parameters = _parameters.Asteroid;

            // ToDo:
        }

        protected DebrisSpec[] GenerateDebris(in AsteroidSpec asteroidSpec, int debrisCount)
        {
            var parameters = _parameters.Debris;

            var specs = new DebrisSpec[debrisCount];
            for (var i = 0; i < debrisCount; ++i)
            {
                var spec = new DebrisSpec();

                // ToDo:

                specs[i] = spec;
            }

            return specs;
        }
    }
}