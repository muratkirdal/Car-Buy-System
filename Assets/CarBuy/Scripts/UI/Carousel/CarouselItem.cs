using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI.Carousel
{
    public class CarouselItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image m_IconImage;
        [SerializeField] private GameObject m_OwnedBadge;
        [SerializeField] private GameObject m_SelectedHighlight;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        [Header("State Colors")]
        [SerializeField] private Color m_DefaultColor = new(0.2f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color m_SelectedColor = new(1f, 0.8f, 0f, 1f);
        [SerializeField] private Color m_OwnedColor = new(0.2f, 0.6f, 0.2f, 1f);

        private VehicleData m_VehicleData;

        public VehicleData VehicleData => m_VehicleData;

        public void Initialize(VehicleData vehicleData)
        {
            m_VehicleData = vehicleData;

            m_IconImage.sprite = vehicleData.ModelInfo.IconSprite;
            m_NameLabel.text = vehicleData.DisplayName;

            SetState(ItemState.Default);
        }

        public void SetState(ItemState state)
        {
            m_OwnedBadge.SetActive(false);
            m_SelectedHighlight.SetActive(false);

            switch (state)
            {
                case ItemState.Default:
                    SetBackgroundColor(m_DefaultColor);
                    break;

                case ItemState.Selected:
                    SetBackgroundColor(m_SelectedColor);
                    m_SelectedHighlight.SetActive(true);
                    break;

                case ItemState.Owned:
                    SetBackgroundColor(m_OwnedColor);
                    m_OwnedBadge.SetActive(true);
                    break;

                case ItemState.SelectedOwned:
                    SetBackgroundColor(m_SelectedColor);
                    m_SelectedHighlight.SetActive(true);
                    m_OwnedBadge.SetActive(true);
                    break;
            }
        }

        private void SetBackgroundColor(Color color)
        {
            m_BackgroundImage.color = color;
        }
    }
}
