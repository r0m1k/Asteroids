using Asteroids.UIEntityData;
using TMPro;

namespace Asteroids.UI
{
    public class ScoreHudView : DataView<ScoreData>
    {
        public TextMeshProUGUI Score;

        protected override void UpdateState(ScoreData data)
        {
            Score.text = data.Score.ToString();
        }
    }
}