using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI.Carousel
{
    public class CarouselItem : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private CarouselItemConfig m_Config;

        [Header("UI References")]
        [SerializeField] private Image m_IconImage;
        [SerializeField] private GameObject m_OwnedBadge;
        [SerializeField] private GameObject m_SelectedHighlight;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

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
                    SetBackgroundColor(m_Config.DefaultColor);
                    break;

                case ItemState.Selected:
                    SetBackgroundColor(m_Config.SelectedColor);
                    m_SelectedHighlight.SetActive(true);
                    break;

                case ItemState.Owned:
                    SetBackgroundColor(m_Config.OwnedColor);
                    m_OwnedBadge.SetActive(true);
                    break;

                case ItemState.SelectedOwned:
                    SetBackgroundColor(m_Config.SelectedColor);
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
