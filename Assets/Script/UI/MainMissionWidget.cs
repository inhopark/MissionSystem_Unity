using System;
using UnityEngine;
using UnityEngine.UI;

namespace MissionSystem
{
    /// Pure view: zero references to Mission/Manager types, only fires events upward.
    public class MainMissionWidget : MonoBehaviour
    {
        public Text titleText;
        public Button leftButton;
        public Text leftButtonText;
        public Button rightButton;
        public Text rightButtonText;

        public event Action<MissionButtonAction> OnMissionButtonAction;

        private void Awake()
        {
            leftButton.onClick.AddListener(() => OnMissionButtonAction?.Invoke(MissionButtonAction.Left));
            rightButton.onClick.AddListener(() => OnMissionButtonAction?.Invoke(MissionButtonAction.Right));
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetWidgetState(MissionState state)
        {
            // Buttons stay visible-but-non-functional during InProgress (relabeled Success/Fail) -
            // an intentional 1:1 port of the UE5 reference's leftover dead-button behavior.
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);

            switch (state)
            {
                case MissionState.Ready:
                    titleText.text = MissionText.WaitForMission;
                    leftButtonText.text = MissionText.AgreeButton;
                    rightButtonText.text = MissionText.DisagreeButton;
                    break;
                case MissionState.InProgress:
                    titleText.text = MissionText.MissionInProgress;
                    leftButtonText.text = MissionText.SuccessButton;
                    rightButtonText.text = MissionText.FailButton;
                    break;
            }
        }
    }
}
