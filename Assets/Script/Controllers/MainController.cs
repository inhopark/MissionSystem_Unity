using UnityEngine;

namespace MissionSystem
{
    /// Owns the 4 UI widget instances and registers them with MissionManager.
    public class MainController : MonoBehaviour
    {
        public MainMissionWidget mainMissionWidget;
        public MissionResultWidget missionResultWidget;
        public HPWidget hpWidget;
        public MissionTimerWidget missionTimerWidget;

        private void Start()
        {
            mainMissionWidget.Hide();
            missionResultWidget.Hide();
            SetDefenseHUDVisible(false);

            MissionManager.Instance.RegisterWidgets(this, mainMissionWidget, missionResultWidget, hpWidget, missionTimerWidget);
        }

        public void SetDefenseHUDVisible(bool visible)
        {
            hpWidget.SetVisible(visible);
            missionTimerWidget.SetVisible(visible);
        }
    }
}
