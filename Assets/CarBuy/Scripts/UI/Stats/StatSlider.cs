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

        private Tween m_FillTween;
        private Tween m_ColorTween;

        private void Awake()
        {
            m_BackgroundImage.color = m_Config.EmptyColor;
        }

        private void OnDisable()
        {
            KillTweens();
        }

        public void SetValue(float value)
        {
            float clampedValue = Mathf.Clamp(value, m_Config.MinValue, m_Config.MaxValue);
            float normalizedValue = CalculateNormalizedValue(clampedValue);

            KillTweens();
            AnimateFill(normalizedValue, clampedValue);
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

        private void AnimateFill(float targetNormalized, float targetValue)
        {
            Color targetColor = Color.Lerp(m_Config.LowValueColor, m_Config.HighValueColor, targetNormalized);

            m_FillTween = DOTween.To(() => m_FillImage.fillAmount, x => m_FillImage.fillAmount = x, targetNormalized, m_Config.AnimationDuration)
                .SetEase(m_Config.FillCurve)
                .OnComplete(() => UpdateText(targetValue));

            m_ColorTween = m_FillImage.DOColor(targetColor, m_Config.AnimationDuration)
                .SetEase(Ease.Linear);

            return;

            void UpdateText(float value)
            {
                m_ValueText.text = $"{Mathf.RoundToInt(value)} / {Mathf.RoundToInt(m_Config.MaxValue)}";
            }
        }

        private void KillTweens()
        {
            m_FillTween?.Kill();
            m_ColorTween?.Kill();
        }
    }
}
