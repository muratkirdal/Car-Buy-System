using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "ColorButtonConfig", menuName = "CarBuy/Configs/Color Button")]
    public class ColorButtonConfig : ScriptableObject
    {
        [Header("Scale Settings")]
        [SerializeField, Range(1f, 1.5f)] private float m_SelectedScale = 1.1f;
        [SerializeField, Range(0.5f, 1f)] private float m_NormalScale = 1.0f;

        public float SelectedScale => m_SelectedScale;
        public float NormalScale => m_NormalScale;
    }
}
