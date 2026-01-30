using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "UIAnimationConfig", menuName = "CarBuy/Configs/UI Animation")]
    public class UIAnimationConfig : ScriptableObject
    {
        [SerializeField, Range(0.1f, 0.5f)] private float m_PopupFadeInDuration = 0.2f;
        [SerializeField, Range(0.1f, 0.5f)] private float m_PopupFadeOutDuration = 0.15f;

        public float PopupFadeInDuration => m_PopupFadeInDuration;
        public float PopupFadeOutDuration => m_PopupFadeOutDuration;
    }
}
