using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class CarouselItem : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private CarouselItemConfig m_Config;

        [Header("UI References")]
        [SerializeField] private RectTransform m_Transform;
        [SerializeField] private Image m_IconImage;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private Button m_Button;
        [SerializeField] private GameObject m_OwnedBadge;
        [SerializeField] private GameObject m_SelectedHighlight;
        [SerializeField] private TextMeshProUGUI m_NameLabel;

        private int m_Index;
        private VehicleData m_VehicleData;

        public event ItemClickedHandler ItemClicked;

        public VehicleData VehicleData => m_VehicleData;

        private void OnEnable()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            ItemClicked?.Invoke(m_Index);
        }

        public void Initialize(VehicleData vehicleData, int index)
        {
            m_VehicleData = vehicleData;
            m_Index = index;

            m_IconImage.sprite = vehicleData.ModelInfo.IconSprite;
            m_NameLabel.text = vehicleData.DisplayName;

            SetState(false, false);
        }

        public void SetState(bool isSelected, bool isOwned)
        {
            m_SelectedHighlight.SetActive(isSelected);
            m_OwnedBadge.SetActive(isOwned);

            Color backgroundColor = isSelected ? m_Config.SelectedColor :
                                    isOwned ? m_Config.OwnedColor :
                                    m_Config.DefaultColor;

            if (isSelected)
                m_Transform.localScale = Vector3.one * 1.1f;
            else
                m_Transform.localScale = Vector3.one * .85f;
            
            SetBackgroundColor(backgroundColor);
        }

        private void SetBackgroundColor(Color color)
        {
            m_BackgroundImage.color = color;
        }
    }

    public delegate void ItemClickedHandler(int index);
}
