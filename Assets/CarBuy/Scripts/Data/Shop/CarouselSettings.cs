using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct CarouselSettings
    {
        [SerializeField, Range(3, 11)] private int m_VisibleItems;
        [SerializeField, Range(0.1f, 1f)] private float m_TransitionDuration;

        public int VisibleItems => m_VisibleItems;
        public float TransitionDuration => m_TransitionDuration;
    }
}