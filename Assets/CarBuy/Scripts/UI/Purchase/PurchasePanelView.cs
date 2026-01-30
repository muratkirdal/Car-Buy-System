using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class PurchasePanelView : MonoBehaviour
    {
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

        private bool m_CurrentIsOwned;

        public event PurchaseClickedHandler PurchaseClicked;

        private void OnEnable()
        {
            m_PurchaseButton.onClick.AddListener(HandlePurchaseClicked);
        }

        private void OnDisable()
        {
            m_PurchaseButton.onClick.RemoveListener(HandlePurchaseClicked);
        }

        public void DisplayVehicle(VehicleData vehicle, bool isOwned)
        {
            m_CurrentIsOwned = isOwned;

            UpdateVehicleName(vehicle.DisplayName);
            UpdatePriceDisplay(vehicle);
            UpdateOwnedState(isOwned);
            UpdatePurchaseButtonText();
        }

        public void SetPurchaseEnabled(bool canAfford)
        {
            if (m_CurrentIsOwned)
            {
                m_PurchaseButton.interactable = false;
                m_PriceText.color = m_Config.AffordableColor;
                return;
            }

            m_PurchaseButton.interactable = canAfford;
            m_PriceText.color = canAfford ? m_Config.AffordableColor : m_Config.UnaffordableColor;
        }

        private void UpdateVehicleName(string name)
        {
            m_VehicleNameText.text = name;
        }

        private void UpdatePriceDisplay(VehicleData vehicle)
        {
            bool isOnSale = vehicle.SalePrice > 0;
            int displayPrice = isOnSale ? vehicle.SalePrice : vehicle.Price;

            m_PriceText.text = string.Format(StringConst.CurrencyFormat, displayPrice);
            m_SaleIndicator.SetActive(isOnSale);

            if (isOnSale)
            {
                string originalPrice = string.Format(StringConst.CurrencyFormat, vehicle.Price);
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

        private void UpdatePurchaseButtonText()
        {
            m_PurchaseButtonText.text = m_CurrentIsOwned ? k_OwnedButtonText : k_PurchaseButtonText;
        }

        private void HandlePurchaseClicked()
        {
            PurchaseClicked?.Invoke();
        }

    }
    public delegate void PurchaseClickedHandler();
}