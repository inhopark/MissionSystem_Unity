using UnityEngine;
using UnityEngine.UI;

namespace MissionSystem
{
    /// The one intentional poller (matches UE5's NativeTick exception) - a timer inherently needs
    /// continuous updates, so it reads MissionManager every frame instead of subscribing to an event.
    public class MissionTimerWidget : MonoBehaviour
    {
        public Slider timerBar;
        public Text timerText;
        public CanvasGroup canvasGroup;

        private void Update()
        {
            float remaining = MissionManager.Instance.GetMissionRemainingTime();
            if (remaining < 0f)
            {
                return;
            }

            float total = MissionManager.Instance.GetMissionTotalDuration();
            if (timerBar != null && total > 0f)
            {
                timerBar.value = remaining / total;
            }

            timerText.text = Mathf.CeilToInt(remaining).ToString();
        }

        public void SetVisible(bool visible)
        {
            canvasGroup.alpha = visible == true ? 1f : 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
