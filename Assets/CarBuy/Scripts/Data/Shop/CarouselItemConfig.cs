using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "CarouselItemConfig", menuName = "CarBuy/Configs/Carousel Item")]
    public class CarouselItemConfig : ScriptableObject
    {
        [Header("State Colors")]
        [SerializeField] private Color m_DefaultColor = new(0.2f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color m_SelectedColor = new(1f, 0.8f, 0f, 1f);
        [SerializeField] private Color m_OwnedColor = new(0.2f, 0.6f, 0.2f, 1f);

        public Color DefaultColor => m_DefaultColor;
        public Color SelectedColor => m_SelectedColor;
        public Color OwnedColor => m_OwnedColor;
    }
}
