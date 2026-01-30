using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI.Purchase
{
    public class PurchasePanelView : MonoBehaviour
    {
        private const string k_CurrencyFormat = "${0:N0}";
        private const string k_PurchaseButtonText = "Purchase";
        private const string k_OwnedButtonText = "Owned";
        private const string k_StrikethroughFormat = "<s>{0}</s>";

        [Header("Config")]
        [SerializeField] private PurchasePanelConfig m_Config;

        [Header("Display")]
        [SerializeField] private TextMeshProUGUI m_VehicleNameText;
        [SerializeField] private TextMeshProUGUI m_PriceText;
        [SerializeField] private TextMeshProUGUI m_OriginalPriceText;
        [SerializeField] private GameObject m_SaleIndicator;

        [Header("Buttons")]
        [SerializeField] private TextMeshProUGUI m_PurchaseButtonText;
        [SerializeField] private Button m_PurchaseButton;
        [SerializeField] private GameObject m_OwnedBadge;

        private VehicleData m_CurrentVehicle;
        private bool m_CurrentIsOwned;
        private int m_PlayerBalance;

        public event PurchaseClickedHandler PurchaseClicked;

        private void OnEnable()
        {
            m_PurchaseButton.onClick.AddListener(HandlePurchaseClicked);
        }

        private void OnDisable()
        {
            m_PurchaseButton.onClick.RemoveListener(HandlePurchaseClicked);
        }

        public void DisplayVehicle(VehicleData vehicle, int playerBalance, bool isOwned)
        {
            m_CurrentVehicle = vehicle;
            m_CurrentIsOwned = isOwned;
            m_PlayerBalance = playerBalance;

            UpdateVehicleName(vehicle.DisplayName);
            UpdatePriceDisplay(vehicle);
            UpdateOwnedState(isOwned);
            UpdatePurchaseButton();
        }

        private void UpdateVehicleName(string name)
        {
            m_VehicleNameText.text = name;
        }

        public void UpdatePlayerBalance(int balance)
        {
            m_PlayerBalance = balance;
            UpdatePurchaseButton();
        }

        private void UpdatePriceDisplay(VehicleData vehicle)
        {
            bool isOnSale = vehicle.SalePrice > 0;
            int displayPrice = isOnSale ? vehicle.SalePrice : vehicle.Price;

            m_PriceText.text = string.Format(k_CurrencyFormat, displayPrice);
            m_SaleIndicator.SetActive(isOnSale);

            if (isOnSale)
            {
                string originalPrice = string.Format(k_CurrencyFormat, vehicle.Price);
                m_OriginalPriceText.text = string.Format(k_StrikethroughFormat, originalPrice);
                m_OriginalPriceText.gameObject.SetActive(true);
            }
            else
            {
                // Show Sellbutton hide other all
                m_OriginalPriceText.gameObject.SetActive(false);
            }
        }

        private void UpdateOwnedState(bool isOwned)
        {
            m_OwnedBadge.SetActive(isOwned);
        }

        private void UpdatePurchaseButton()
        {
            m_PurchaseButtonText.text = m_CurrentIsOwned ? k_OwnedButtonText : k_PurchaseButtonText;

            if (m_CurrentIsOwned)
            {
                m_PurchaseButton.interactable = false;
                return;
            }

            int effectivePrice = m_CurrentVehicle.SalePrice > 0 ? m_CurrentVehicle.SalePrice : m_CurrentVehicle.Price;
            bool canAfford = m_PlayerBalance >= effectivePrice;
            SetPurchaseEnabled(canAfford);
        }

        private void SetPurchaseEnabled(bool enabled)
        {
            m_PurchaseButton.interactable = enabled;
            m_PriceText.color = enabled ? m_Config.AffordableColor : m_Config.UnaffordableColor;
        }

        private void HandlePurchaseClicked()
        {
            PurchaseClicked?.Invoke();
        }

        public delegate void PurchaseClickedHandler();
    }
}
