using System;
using UnityEngine;
using UnityEngine.UI;

namespace MissionSystem
{
    /// One widget reused for both Succeeded and Failed outcomes (only the title text swaps).
    public class MissionResultWidget : MonoBehaviour
    {
        public Text titleText;
        public Button confirmButton;

        public event Action OnConfirmed;

        private void Awake()
        {
            confirmButton.onClick.AddListener(() => OnConfirmed?.Invoke());
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetResultState(MissionState state)
        {
            titleText.text = state == MissionState.Succeeded ? MissionText.MissionSuccess : MissionText.MissionFailed;
        }
    }
}
