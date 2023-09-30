using Asteroids.ECS.Entities;
using Asteroids.Services.ECS;
using Asteroids.UIEntityData;
using ECS;
using Services;
using StateMachine;
using System.Collections;
using Asteroids.Services;
using UnityEngine;

namespace Asteroids.StateMachine
{
    public class PlayState : State
    {
        private readonly IServiceProvider _services;
        private readonly IUpdateService _updateService;
        private readonly IEntityWorldService _entityWorldService;
        private readonly IDataService _dataService;
        private readonly IUIService _uiService;
        private readonly ICoroutineService _coroutineService;
        private readonly IInputService _inputService;

        public PlayState(IStateMachine stateMachine, IServiceProvider services) : base(stateMachine)
        {
            _services = services;
            _updateService = _services.Get<IUpdateService>();
            _entityWorldService = _services.Get<IEntityWorldService>();
            _dataService = _services.Get<IDataService>();
            _uiService = _services.Get<IUIService>();
            _coroutineService = _services.Get<ICoroutineService>();
            _inputService = _services.Get<IInputService>();
        }

        public override void Enter()
        {
            // ToDo: check if need to move to spawn/clean state?
            UpdateGameData(true);
            HideLoadScreen();
            EnableInput();

            _updateService.Update += UpdateHandler;
        }

        public override void Exit()
        {
            DisableInput();
            ShowLoadScreen();
            UpdateGameData(false);

            _updateService.Update -= UpdateHandler;
        }

        private void UpdateGameData(bool isPlaying)
        {
            // ToDo: check if need to move to spawn/clean state?
            var gameData = _dataService.GetFirstOrCreate<GameData>();
            gameData.Data.PlayerShipEntityId = isPlaying ? _entityWorldService.World.FindFirst<PlayerShipEntity>().Id : Constants.InvalidEntityId;
            gameData.Data.IsGamePlaying = isPlaying;
            gameData.Notify();
        }

        private void UpdateHandler()
        {
            var playerShipEntity = _entityWorldService.World.FindFirst<PlayerShipEntity>();
            if (playerShipEntity != null) return;

            _updateService.Update -= UpdateHandler;

            _uiService.HideHud();

            _coroutineService.StartCoroutine(ShowPlayerDeathDialogWithDelay());
        }

        private IEnumerator ShowPlayerDeathDialogWithDelay()
        {
            DisableInput();

            // ToDo: move 'const' to parameters
            yield return new WaitForSeconds(1.5f);

            _uiService.ShowPlayerDeathDialog(RestartClickHandler);
        }

        private void RestartClickHandler()
        {
            _stateMachine.Enter<CleanUpState>();
        }

        private void ShowLoadScreen()
        {
            var loadScreen = _services.Get<ILoadScreenService>();
            loadScreen.ShowImmediately();
        }

        private void HideLoadScreen()
        {
            var loadScreen = _services.Get<ILoadScreenService>();
            loadScreen.HideImmediately();
        }

        private void EnableInput()
        {
            _inputService.Enable();
        }

        private void DisableInput()
        {
            _inputService.Disable();
        }
    }
}