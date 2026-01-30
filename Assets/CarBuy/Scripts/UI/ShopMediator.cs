using System.Collections.Generic;
using UnityEngine;
using CarBuy.Data;
using CarBuy.Services;
using CarBuy.UI.Carousel;
using CarBuy.UI.Common;
using CarBuy.UI.HUD;
using CarBuy.UI.Purchase;
using CarBuy.UI.Views;
using CarBuy.Vehicle;

namespace CarBuy.UI
{
    public class ShopMediator : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private VehicleLibrary m_VehicleLibrary;
        [SerializeField] private CurrencyConfig m_CurrencyConfig;

        [Header("UI Views")]
        [SerializeField] private ConfirmationPopup m_ConfirmationPopup;
        [SerializeField] private CarouselView m_CarouselView;
        [SerializeField] private CurrencyHudView m_CurrencyHudView;
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
        }

        public void Initialize()
        {
            var playerData = new PlayerShopData(m_CurrencyConfig.StartingBalance);

            m_VehicleService = new VehicleService(m_VehicleLibrary, playerData);
            m_CurrencyService = new CurrencyService(playerData);
            m_TransactionService = new TransactionService(m_VehicleService, m_CurrencyService, playerData);

            m_State = new ShopUIState
            {
                CurrentVehicleIndex = 0,
                SelectedColorIndex = 0,
                CurrentVehicle = null
            };

            IReadOnlyList<VehicleData> vehicles = m_VehicleLibrary.Vehicles;

            m_State.CurrentVehicle = vehicles[0];

            m_CarouselView.Initialize(vehicles);
            m_CurrencyHudView.Initialize(m_CurrencyService);

            m_CarouselView.VehicleSelected += HandleVehicleSelected;
            m_StatsView.ColorSelected += HandleColorSelected;
            m_PurchaseView.PurchaseClicked += HandlePurchaseRequest;
            m_CurrencyService.BalanceChanged += HandleBalanceChanged;
            m_TransactionService.PurchaseCompleted += HandlePurchaseCompleted;
        }

        private void HandleVehicleSelected(int index, VehicleData vehicle)
        {
            m_State.CurrentVehicle = vehicle;
            m_State.CurrentVehicleIndex = index;

            bool isOwned = m_VehicleService.IsVehicleOwned(vehicle.Id);

            m_StatsView.DisplayVehicle(vehicle);
            m_PurchaseView.DisplayVehicle(vehicle, isOwned);
            m_PurchaseView.SetPurchaseEnabled(CanAffordCurrentVehicle());
            m_VehicleShowcase.DisplayVehicle(vehicle, m_State.SelectedColorIndex);
        }

        private void HandleColorSelected(int colorIndex, VehicleColorOption selectedColorOption)
        {
            m_State.SelectedColorIndex = colorIndex;
            m_VehicleShowcase.SetColor(selectedColorOption);
        }

        private void HandleBalanceChanged(int newBalance)
        {
            m_PurchaseView.SetPurchaseEnabled(CanAffordCurrentVehicle());
        }

        private bool CanAffordCurrentVehicle()
        {
            VehicleData vehicle = m_State.CurrentVehicle;
            int effectivePrice = vehicle.SalePrice > 0 ? vehicle.SalePrice : vehicle.Price;
            return m_CurrencyService.CurrentBalance >= effectivePrice;
        }

        private void HandlePurchaseRequest()
        {
            VehicleData vehicle = m_State.CurrentVehicle;

            m_ConfirmationPopup.Show(vehicle.DisplayName, vehicle.Price, confirmed =>
            {
                if (confirmed)
                {
                    ProcessPurchase(vehicle);
                }
            });
        }

        private void ProcessPurchase(VehicleData vehicle)
        {
            m_TransactionService.PurchaseVehicle(vehicle.Id, m_State.SelectedColorIndex);
        }

        private void HandlePurchaseCompleted(PurchaseTransaction transaction)
        {
            m_CarouselView.MarkItemAsOwned(transaction.VehicleId);

            VehicleData currentVehicle = m_State.CurrentVehicle;

            m_StatsView.DisplayVehicle(currentVehicle);
            m_PurchaseView.DisplayVehicle(currentVehicle, true);
            m_PurchaseView.SetPurchaseEnabled(false);
        }

        public void OpenShop()
        {
            m_CarouselView.SelectIndex(0);

            VehicleData firstVehicle = m_State.CurrentVehicle;
            bool isOwned = m_VehicleService.IsVehicleOwned(firstVehicle.Id);

            m_StatsView.DisplayVehicle(firstVehicle);
            m_PurchaseView.DisplayVehicle(firstVehicle, isOwned);
            m_PurchaseView.SetPurchaseEnabled(CanAffordCurrentVehicle());
            m_VehicleShowcase.DisplayVehicle(firstVehicle, 0);
        }

        public void CloseShop()
        {
            m_ConfirmationPopup.ForceClose();
        }
    }
}
