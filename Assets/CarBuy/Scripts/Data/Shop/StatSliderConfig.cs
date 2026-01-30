using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "StatSliderConfig", menuName = "CarBuy/Configs/Stat Slider")]
    public class StatSliderConfig : ScriptableObject
    {
        [Header("Value Range")]
        [SerializeField] private float m_MinValue = 0f;
        [SerializeField] private float m_MaxValue = 100f;

        [Header("Animation")]
        [SerializeField] private float m_AnimationDuration = 0.4f;
        [SerializeField] private AnimationCurve m_FillCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Colors")]
        [SerializeField] private Color m_EmptyColor = new(0.2f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color m_LowValueColor = new(1f, 1f, 0f, 1f);
        [SerializeField] private Color m_HighValueColor = new(0f, 1f, 0f, 1f);

        public float MinValue => m_MinValue;
        public float MaxValue => m_MaxValue;
        public float AnimationDuration => m_AnimationDuration;
        public AnimationCurve FillCurve => m_FillCurve;
        public Color EmptyColor => m_EmptyColor;
        public Color LowValueColor => m_LowValueColor;
        public Color HighValueColor => m_HighValueColor;
    }
}
