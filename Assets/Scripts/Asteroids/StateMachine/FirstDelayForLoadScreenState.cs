using System.Collections;
using Asteroids.Services;
using Services;
using StateMachine;
using UnityEngine;

namespace Asteroids.StateMachine
{
    public class FirstDelayForLoadScreenState : State
    {
        private const string FirstTimeLongLoadScreen = "FirstTimeLongLoadScreen";

        private readonly IServiceProvider _services;
        private readonly IPersistentDataService _persistentDataService;

        public FirstDelayForLoadScreenState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
            _persistentDataService = _services.Get<IPersistentDataService>();
        }

        public override void Enter()
        {
            var coroutineService = _services.Get<ICoroutineService>();

            if (_persistentDataService.GetBool(FirstTimeLongLoadScreen))
                GoNextStep();
            else
                coroutineService.StartCoroutine(DelayToKeepLoadscreen());
        }

        public override void Exit()
        {
        }

        private IEnumerator DelayToKeepLoadscreen()
        {
            yield return new WaitForSeconds(3f);

            _persistentDataService.Set(FirstTimeLongLoadScreen, true);
            GoNextStep();
        }

        private void GoNextStep()
        {
            _stateMachine.Enter<SpawnState>();
        }
    }
}