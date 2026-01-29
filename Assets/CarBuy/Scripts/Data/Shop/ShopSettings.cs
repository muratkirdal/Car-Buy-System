using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "ShopSettings", menuName = "CarBuy/Shop Settings")]
    public class ShopSettings : ScriptableObject
    {
        [SerializeField] private CarouselSettings m_Carousel;
        [SerializeField] private VehicleDisplaySettings m_VehicleDisplay;
        [SerializeField] private UIAnimationSettings m_UIAnimation;
        [SerializeField] private CurrencySettings m_Currency;

        public CarouselSettings Carousel => m_Carousel;
        public VehicleDisplaySettings VehicleDisplay => m_VehicleDisplay;
        public UIAnimationSettings UIAnimation => m_UIAnimation;
        public CurrencySettings Currency => m_Currency;
    }
}
