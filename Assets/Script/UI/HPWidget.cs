using UnityEngine;
using UnityEngine.UI;

namespace MissionSystem
{
    /// Pure display: subscribes to the player's OnHPChanged event, smooths the bar toward the target.
    public class HPWidget : MonoBehaviour
    {
        public Slider hpBar;
        public Text hpText;
        public CanvasGroup canvasGroup;

        [SerializeField] private float barInterpSpeed = 2f;

        private float _targetPercent = 1f;

        public void Bind(PlayerCharacter player)
        {
            player.OnHPChanged += SetHP;
            SetHP(player.CurrentHP, player.MaxHP);
        }

        public void SetHP(float current, float max)
        {
            _targetPercent = max > 0f ? current / max : 0f;
            hpText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
        }

        private void Update()
        {
            hpBar.value = Mathf.Lerp(hpBar.value, _targetPercent, 1f - Mathf.Exp(-barInterpSpeed * Time.deltaTime));
        }

        public void SetVisible(bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
