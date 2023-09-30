using System;
using Asteroids.UIEntityData;
using TMPro;

namespace Asteroids.UI
{
    public class PlayerDeathDialogView : DataView<ScoreData>
    {
        public Action RestartCallback;

        public TextMeshProUGUI ScoreText;

        protected override void UpdateState(ScoreData data)
        {
            ScoreText.text = data.Score.ToString();
        }

        public void RestartButtonClick()
        {
            _uiRoot.PopView();

            RestartCallback?.Invoke();
            RestartCallback = null;
        }
    }
}