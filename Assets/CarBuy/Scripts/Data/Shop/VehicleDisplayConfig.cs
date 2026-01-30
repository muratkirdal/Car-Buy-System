using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "VehicleDisplayConfig", menuName = "CarBuy/Configs/Vehicle Display")]
    public class VehicleDisplayConfig : ScriptableObject
    {
        [Header("Platform")]
        [SerializeField, Range(0f, 30f)] private float m_PlatformRotationSpeed = 10f;
        [SerializeField, Range(0.1f, 2f)] private float m_TransitionDuration = 0.5f;

        public float PlatformRotationSpeed => m_PlatformRotationSpeed;
        public float TransitionDuration => m_TransitionDuration;
    }
}
