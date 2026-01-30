using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class TradePanelView : MonoBehaviour
    {
        private const string k_BuyButtonText = "Purchase";
        private const string k_SellButtonText = "Sell";

        [Header("Config")]
        [SerializeField] private TradePanelConfig m_Config;

        [Header("Display")]
        [SerializeField] private TextMeshProUGUI m_VehicleNameText;
        [SerializeField] private TextMeshProUGUI m_PriceText;

        [Header("Buttons")]
        [SerializeField] private TextMeshProUGUI m_ActionButtonText;
        [SerializeField] private Button m_ActionButton;

        private bool m_CurrentIsOwned;

        public event BuyClickedHandler BuyClicked;
        public event SellClickedHandler SellClicked;

        private void OnEnable()
        {
            m_ActionButton.onClick.AddListener(HandleActionClicked);
        }

        private void OnDisable()
        {
            m_ActionButton.onClick.RemoveListener(HandleActionClicked);
        }

        public void DisplayVehicle(VehicleData vehicle, bool isOwned)
        {
            m_CurrentIsOwned = isOwned;

            UpdateVehicleName(vehicle.DisplayName);
            UpdatePriceDisplay(vehicle, isOwned);
            UpdateButtonState();
        }

        public void SetBuyEnabled(bool canAfford)
        {
            if (m_CurrentIsOwned)
            {
                return;
            }

            m_ActionButton.interactable = canAfford;
            m_PriceText.color = canAfford ? m_Config.AffordableColor : m_Config.UnaffordableColor;
        }

        private void UpdateVehicleName(string name)
        {
            m_VehicleNameText.text = name;
        }

        private void UpdatePriceDisplay(VehicleData vehicle, bool isOwned)
        {
            int price = isOwned ? vehicle.SalePrice : vehicle.Price;
            m_PriceText.text = string.Format(StringConst.CurrencyFormat, price);
        }

        private void UpdateButtonState()
        {
            m_ActionButtonText.text = m_CurrentIsOwned ? k_SellButtonText : k_BuyButtonText;

            if (m_CurrentIsOwned)
            {
                m_ActionButton.interactable = true;
                m_PriceText.color = m_Config.SellColor;
            }
        }

        private void HandleActionClicked()
        {
            if (m_CurrentIsOwned)
            {
                SellClicked?.Invoke();
            }
            else
            {
                BuyClicked?.Invoke();
            }
        }

    }

    public delegate void BuyClickedHandler();
    public delegate void SellClickedHandler();
}
