using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class StatSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image m_FillImage;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private TextMeshProUGUI m_ValueText;

        [Header("Configuration")]
        [SerializeField] private StatSliderConfig m_Config;

        private float m_CurrentValue;
        private Tween m_FillTween;
        private Tween m_ColorTween;

        private void Awake()
        {
            InitializeBackground();
        }

        private void OnDisable()
        {
            KillTweens();
        }

        private void OnDestroy()
        {
            KillTweens();
        }

        public void SetValue(float value)
        {
            m_CurrentValue = Mathf.Clamp(value, m_Config.MinValue, m_Config.MaxValue);
            float normalizedValue = CalculateNormalizedValue(m_CurrentValue);

            KillTweens();
            AnimateFill(normalizedValue);
        }

        private float CalculateNormalizedValue(float clampedValue)
        {
            float range = m_Config.MaxValue - m_Config.MinValue;

            if (range <= 0f)
            {
                return 0f;
            }

            return (clampedValue - m_Config.MinValue) / range;
        }

        private void InitializeBackground()
        {
            m_BackgroundImage.color = m_Config.EmptyColor;
        }

        private void ApplyFillImmediate(float normalizedValue)
        {
            m_FillImage.fillAmount = normalizedValue;
            m_FillImage.color = EvaluateGradient(normalizedValue);
            UpdateText();
        }

        private void AnimateFill(float targetNormalized)
        {
            UpdateText();

            float startFill = m_FillImage.fillAmount;
            Color targetColor = EvaluateGradient(targetNormalized);

            m_FillTween = DOTween.To(() => startFill, x => m_FillImage.fillAmount = x, targetNormalized, m_Config.AnimationDuration)
                .SetEase(m_Config.FillCurve)
                .OnComplete(UpdateText);

            m_ColorTween = m_FillImage.DOColor(targetColor, m_Config.AnimationDuration)
                .SetEase(Ease.Linear);
        }

        private Color EvaluateGradient(float normalizedValue)
        {
            return Color.Lerp(m_Config.LowValueColor, m_Config.HighValueColor, normalizedValue);
        }

        private void UpdateText()
        {
            int displayValue = Mathf.RoundToInt(m_CurrentValue);
            int displayMax = Mathf.RoundToInt(m_Config.MaxValue);
            m_ValueText.text = $"{displayValue} / {displayMax}";
        }

        private void KillTweens()
        {
            m_FillTween?.Kill();
            m_ColorTween?.Kill();
        }
    }
}
