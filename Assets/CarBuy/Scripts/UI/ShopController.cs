using System.Collections.Generic;
using UnityEngine;
using CarBuy.Data;
using CarBuy.Services;
using CarBuy.UI.Carousel;
using CarBuy.UI.Common;
using CarBuy.UI.Purchase;
using CarBuy.UI.Views;
using CarBuy.Vehicle;

namespace CarBuy.UI
{
    public class ShopController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private VehicleLibrary m_VehicleLibrary;
        [SerializeField] private ShopSettings m_Settings;

        [Header("UI Views")]
        [SerializeField] private ConfirmationPopup m_ConfirmationPopup;
        [SerializeField] private CarouselView m_CarouselView;
        [SerializeField] private PurchasePanelView m_PurchaseView;
        [SerializeField] private StatsPanelView m_StatsView;

        [Header("3D Display")]
        [SerializeField] private VehicleShowcase m_VehicleShowcase;

        private IVehicleService m_VehicleService;
        private ICurrencyService m_CurrencyService;
        private ITransactionService m_TransactionService;
        private ShopUIState m_State;

        private void OnDisable()
        {
            m_CarouselView.VehicleSelected -= HandleVehicleSelected;
            m_StatsView.ColorSelected -= HandleColorSelected;
            m_PurchaseView.PurchaseClicked -= HandlePurchaseRequest;
            m_CurrencyService.BalanceChanged -= HandleBalanceChanged;
            m_TransactionService.PurchaseCompleted -= HandlePurchaseCompleted;
            m_TransactionService.PurchaseFailed -= HandleTransactionServicePurchaseFailed;
        }

        public void Initialize()
        {
            var playerData = new PlayerShopData(m_Settings.Currency.StartingBalance);

            m_VehicleService = new VehicleService(m_VehicleLibrary, playerData);
            m_CurrencyService = new CurrencyService(playerData);
            m_TransactionService = new TransactionService(m_VehicleService, m_CurrencyService, playerData);

            ApplySettings();

            m_State = new ShopUIState
            {
                CurrentVehicleIndex = 0,
                SelectedColorIndex = 0,
                IsPopupOpen = false,
                IsProcessingPurchase = false,
                CurrentVehicle = null
            };

            IReadOnlyList<VehicleData> vehicles = m_VehicleLibrary.Vehicles;

            m_State.CurrentVehicle = vehicles[0];

            m_CarouselView.ApplySettings(m_Settings.Carousel);
            m_CarouselView.Initialize(vehicles);

            m_CarouselView.VehicleSelected += HandleVehicleSelected;
            m_StatsView.ColorSelected += HandleColorSelected;
            m_PurchaseView.PurchaseClicked += HandlePurchaseRequest;
            m_CurrencyService.BalanceChanged += HandleBalanceChanged;
            m_TransactionService.PurchaseCompleted += HandlePurchaseCompleted;
            m_TransactionService.PurchaseFailed += HandleTransactionServicePurchaseFailed;
        }

        private void ApplySettings()
        {
            m_VehicleShowcase.ApplySettings(m_Settings.VehicleDisplay);
            m_StatsView.ApplySettings(m_Settings.UIAnimation);
            m_ConfirmationPopup.ApplySettings(m_Settings.UIAnimation);
        }

        private void HandleVehicleSelected(int index, VehicleData vehicle)
        {
            m_State.CurrentVehicle = vehicle;
            m_State.CurrentVehicleIndex = index;

            bool isOwned = m_VehicleService.IsVehicleOwned(vehicle.Id);
            int playerBalance = m_CurrencyService.CurrentBalance;

            m_StatsView.DisplayVehicle(vehicle);
            m_PurchaseView.DisplayVehicle(vehicle, playerBalance, isOwned);
            m_VehicleShowcase.DisplayVehicle(vehicle, m_State.SelectedColorIndex);
        }

        private void HandleColorSelected(int colorIndex, VehicleColorOption selectedColorOption)
        {
            m_State.SelectedColorIndex = colorIndex;
            m_VehicleShowcase.SetColor(selectedColorOption);
        }

        private void HandleBalanceChanged(int newBalance)
        {
            m_PurchaseView.SetBalance(newBalance);
        }

        private void HandlePurchaseRequest()
        {
            VehicleData vehicle = m_State.CurrentVehicle;
            m_State.IsPopupOpen = true;

            m_ConfirmationPopup.Show(vehicle.DisplayName, vehicle.Price, confirmed =>
            {
                m_State.IsPopupOpen = false;

                if (!confirmed)
                {
                    return;
                }

                ProcessPurchase(vehicle);
            });
        }

        private void ProcessPurchase(VehicleData vehicle)
        {
            m_State.IsProcessingPurchase = true;

            TransactionResult result = m_TransactionService.PurchaseVehicle(vehicle.Id, m_State.SelectedColorIndex);

            if (result == TransactionResult.Success)
            {
                m_PurchaseView.DisplayVehicle(vehicle, m_CurrencyService.CurrentBalance, true);
            }

            m_State.IsProcessingPurchase = false;
        }

        private void HandleTransactionServicePurchaseFailed(PurchaseTransaction transaction, string errorMessage)
        {
            Debug.LogError($"[ShopController] Purchase failed for vehicle '{transaction.VehicleId}': {errorMessage}");

            m_State.IsProcessingPurchase = false;
        }

        private void HandlePurchaseCompleted(PurchaseTransaction transaction)
        {
            m_CarouselView.MarkItemAsOwned(transaction.VehicleId);

            VehicleData currentVehicle = m_State.CurrentVehicle;
            int playerBalance = m_CurrencyService.CurrentBalance;

            m_StatsView.DisplayVehicle(currentVehicle);

            m_PurchaseView.DisplayVehicle(currentVehicle, playerBalance, true);
        }

        public void OpenShop()
        {
            m_CarouselView.SelectIndex(0);

            VehicleData firstVehicle = m_State.CurrentVehicle;

            bool isOwned = m_VehicleService.IsVehicleOwned(firstVehicle.Id);
            int playerBalance = m_CurrencyService.CurrentBalance;

            m_StatsView.DisplayVehicle(firstVehicle);
            m_PurchaseView.DisplayVehicle(firstVehicle, playerBalance, isOwned);
            m_VehicleShowcase.DisplayVehicle(firstVehicle, 0);
        }

        public void CloseShop()
        {
            if (m_State.IsPopupOpen)
            {
                m_ConfirmationPopup.ForceClose();
            }
        }
    }
}
