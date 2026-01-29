using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace CarBuy.UI.Stats
{
    public class StatSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image m_FillImage;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private TextMeshProUGUI m_ValueText;

        [Header("Value Range")]
        [SerializeField] private float m_MinValue = 0f;
        [SerializeField] private float m_MaxValue = 100f;

        [Header("Animation")]
        [SerializeField] private float m_AnimationDuration = 0.4f;
        [SerializeField]private AnimationCurve m_FillCurve;

        [Header("Colors")]
        [SerializeField] private Color m_EmptyColor = new(0.2f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color m_LowValueColor = new(1f, 1f, 0f, 1f);
        [SerializeField] private Color m_HighValueColor = new(0f, 1f, 0f, 1f);

        private float m_CurrentValue;
        private Tween m_FillTween;
        private Tween m_ColorTween;

        public void SetAnimationDuration(float duration)
        {
            m_AnimationDuration = duration;
        }

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
            m_CurrentValue = Mathf.Clamp(value, m_MinValue, m_MaxValue);
            float normalizedValue = CalculateNormalizedValue(m_CurrentValue);

            KillTweens();
            AnimateFill(normalizedValue);
        }

        private float CalculateNormalizedValue(float clampedValue)
        {
            float range = m_MaxValue - m_MinValue;

            if (range <= 0f)
            {
                return 0f;
            }

            return (clampedValue - m_MinValue) / range;
        }

        private void InitializeBackground()
        {
            m_BackgroundImage.color = m_EmptyColor;
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

            m_FillTween = DOTween.To(() => startFill, x => m_FillImage.fillAmount = x, targetNormalized, m_AnimationDuration)
                .SetEase(m_FillCurve)
                .OnComplete(UpdateText);

            m_ColorTween = m_FillImage.DOColor(targetColor, m_AnimationDuration)
                .SetEase(Ease.Linear);
        }

        private Color EvaluateGradient(float normalizedValue)
        {
            return Color.Lerp(m_LowValueColor, m_HighValueColor, normalizedValue);
        }

        private void UpdateText()
        {
            int displayValue = Mathf.RoundToInt(m_CurrentValue);
            int displayMax = Mathf.RoundToInt(m_MaxValue);
            m_ValueText.text = $"{displayValue} / {displayMax}";
        }

        private void KillTweens()
        {
            m_FillTween?.Kill();
            m_ColorTween?.Kill();
        }
    }
}
