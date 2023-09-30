using Infrastructure;
using Services;
using System;

namespace Asteroids.Services
{
    public interface IUIService : IService, IRequireInitialize, IRequireCleanUp
    {
        void CreateHud();

        void ShowHud();
        void HideHud();

        void PopView();
        void PopAllViews();
        void ShowPlayerDeathDialog(Action restartClickCallback);
    }
}