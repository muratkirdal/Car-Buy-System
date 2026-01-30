using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "CarouselConfig", menuName = "CarBuy/Configs/Carousel")]
    public class CarouselConfig : ScriptableObject
    {
        [SerializeField, Range(3, 11)] private int m_VisibleItems = 7;
        [SerializeField, Range(0.1f, 1f)] private float m_TransitionDuration = 0.3f;
        [SerializeField] private AnimationCurve m_TransitionCurve;

        public int VisibleItems => m_VisibleItems;
        public float TransitionDuration => m_TransitionDuration;
        public AnimationCurve TransitionCurve => m_TransitionCurve;
    }
}
