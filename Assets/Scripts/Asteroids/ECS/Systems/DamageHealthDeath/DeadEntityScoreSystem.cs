using Asteroids.ECS.Components;
using Asteroids.Services;
using Asteroids.UIEntityData;
using ECS;

namespace Asteroids.ECS.Systems
{
    public class DeadEntityScoreSystem : EntitySystem, IEntitySystemRequireFixedUpdate
    {
        private readonly IDataService _dataService;

        public DeadEntityScoreSystem(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            var scoreData = _dataService.GetFirstOrCreate<ScoreData>();
            var changed = false;

            var entities = World.FilterByComponents<IsDeadComponent, ScoreComponent>();
            foreach (var entity in entities)
            {
                var score = entity.GetComponent<ScoreComponent>();

                scoreData.Data.Score += score.Value;
                changed = true;
            }

            if (changed) scoreData.Notify();
        }
    }
}