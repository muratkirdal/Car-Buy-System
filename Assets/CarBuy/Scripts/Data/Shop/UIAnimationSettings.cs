using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct UIAnimationSettings
    {
        [SerializeField, Range(0.1f, 1f)] private float m_StatsFillDuration;
        [SerializeField, Range(0.1f, 0.5f)] private float m_PopupFadeInDuration;
        [SerializeField, Range(0.1f, 0.5f)] private float m_PopupFadeOutDuration;

        public float StatsFillDuration => m_StatsFillDuration;
        public float PopupFadeInDuration => m_PopupFadeInDuration;
        public float PopupFadeOutDuration => m_PopupFadeOutDuration;
    }
}