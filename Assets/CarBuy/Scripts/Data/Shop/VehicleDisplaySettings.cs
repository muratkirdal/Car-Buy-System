using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct VehicleDisplaySettings
    {
        [SerializeField, Range(0f, 30f)] private float m_PlatformRotationSpeed;
        [SerializeField, Range(0.1f, 2f)] private float m_TransitionDuration;

        public float PlatformRotationSpeed => m_PlatformRotationSpeed;
        public float TransitionDuration => m_TransitionDuration;
    }
}