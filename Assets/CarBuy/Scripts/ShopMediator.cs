using System.Collections.Generic;
using UnityEngine;
using CarBuy.Data;
using CarBuy.Services;
using CarBuy.UI;
using CarBuy.Vehicle;

namespace CarBuy
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
        [SerializeField] private TradePanelView m_TradeView;
        [SerializeField] private StatsPanelView m_StatsView;

        [Header("3D Display")]
        [SerializeField] private VehicleShowcase m_VehicleShowcase;

        private IVehicleService m_VehicleService;
        private ICurrencyService m_CurrencyService;
        private ITransactionService m_TransactionService;
        private ShopUIState m_State;
        private VehicleData m_PendingVehicle;

        private void OnDisable()
        {
            m_CarouselView.VehicleSelected -= HandleVehicleSelected;
            m_StatsView.ColorSelected -= HandleColorSelected;
            m_TradeView.BuyClicked -= HandleBuyRequest;
            m_TradeView.SellClicked -= HandleSellRequest;
            m_CurrencyService.BalanceChanged -= HandleBalanceChanged;
            m_TransactionService.PurchaseCompleted -= HandleBuyCompleted;
            m_TransactionService.SellCompleted -= HandleSellCompleted;
        }

        public void Initialize()
        {
            var playerData = new PlayerShopData(m_CurrencyConfig.StartingBalance);

            m_VehicleService = new VehicleService(m_VehicleLibrary, playerData);
            m_CurrencyService = new CurrencyService(playerData);
            m_TransactionService = new TransactionService(m_VehicleService, m_CurrencyService, playerData);

            m_State = new ShopUIState
            {
                SelectedColorIndex = 0,
                CurrentVehicle = null
            };

            IReadOnlyList<VehicleData> vehicles = m_VehicleLibrary.Vehicles;

            m_State.CurrentVehicle = vehicles[0];

            m_CarouselView.Initialize(vehicles);
            m_CurrencyHudView.Initialize(m_CurrencyService);

            m_CarouselView.VehicleSelected += HandleVehicleSelected;
            m_StatsView.ColorSelected += HandleColorSelected;
            m_TradeView.BuyClicked += HandleBuyRequest;
            m_TradeView.SellClicked += HandleSellRequest;
            m_CurrencyService.BalanceChanged += HandleBalanceChanged;
            m_TransactionService.PurchaseCompleted += HandleBuyCompleted;
            m_TransactionService.SellCompleted += HandleSellCompleted;
        }

        public void OpenShop()
        {
            m_CarouselView.SelectIndex(0);

            VehicleData firstVehicle = m_State.CurrentVehicle;
            bool isOwned = m_VehicleService.IsVehicleOwned(firstVehicle.Id);

            m_VehicleShowcase.DisplayVehicle(firstVehicle, 0);
            m_StatsView.DisplayVehicle(firstVehicle);
            m_TradeView.DisplayVehicle(firstVehicle, isOwned);
            m_TradeView.SetBuyEnabled(CanAffordCurrentVehicle());
        }

        public void CloseShop()
        {
            m_ConfirmationPopup.ForceClose();
        }

        private void HandleVehicleSelected(int index, VehicleData vehicle)
        {
            m_State.CurrentVehicle = vehicle;

            bool isOwned = m_VehicleService.IsVehicleOwned(vehicle.Id);

            m_StatsView.DisplayVehicle(vehicle);
            m_TradeView.DisplayVehicle(vehicle, isOwned);
            m_TradeView.SetBuyEnabled(CanAffordCurrentVehicle());
            m_VehicleShowcase.DisplayVehicle(vehicle, m_State.SelectedColorIndex);
        }

        private void HandleColorSelected(int colorIndex, VehicleColorOption selectedColorOption)
        {
            m_State.SelectedColorIndex = colorIndex;
            m_VehicleShowcase.SetColor(selectedColorOption);
        }

        private void HandleBalanceChanged(int newBalance)
        {
            m_TradeView.SetBuyEnabled(CanAffordCurrentVehicle());
        }

        private bool CanAffordCurrentVehicle()
        {
            VehicleData vehicle = m_State.CurrentVehicle;
            return m_CurrencyService.CurrentBalance >= vehicle.Price;
        }

        private void HandleBuyRequest()
        {
            m_PendingVehicle = m_State.CurrentVehicle;

            m_ConfirmationPopup.Confirmed += HandleBuyConfirmed;
            m_ConfirmationPopup.Show(m_PendingVehicle.DisplayName, m_PendingVehicle.Price);
        }

        private void HandleBuyConfirmed()
        {
            m_ConfirmationPopup.Confirmed -= HandleBuyConfirmed;
            m_TransactionService.PurchaseVehicle(m_PendingVehicle.Id, m_State.SelectedColorIndex);
        }

        private void HandleBuyCompleted(VehicleTransaction transaction)
        {
            m_CarouselView.MarkItemAsOwned(transaction.VehicleId);

            VehicleData currentVehicle = m_State.CurrentVehicle;

            m_StatsView.DisplayVehicle(currentVehicle);
            m_TradeView.DisplayVehicle(currentVehicle, true);
            m_TradeView.SetBuyEnabled(false);
        }

        private void HandleSellRequest()
        {
            m_PendingVehicle = m_State.CurrentVehicle;

            m_ConfirmationPopup.Confirmed += HandleSellConfirmed;
            m_ConfirmationPopup.ShowSell(m_PendingVehicle.DisplayName, m_PendingVehicle.SalePrice);
        }

        private void HandleSellConfirmed()
        {
            m_ConfirmationPopup.Confirmed -= HandleSellConfirmed;
            m_TransactionService.SellVehicle(m_PendingVehicle.Id);
        }

        private void HandleSellCompleted(VehicleTransaction transaction)
        {
            m_CarouselView.MarkItemAsNotOwned(transaction.VehicleId);

            VehicleData currentVehicle = m_State.CurrentVehicle;

            m_StatsView.DisplayVehicle(currentVehicle);
            m_TradeView.DisplayVehicle(currentVehicle, false);
            m_TradeView.SetBuyEnabled(CanAffordCurrentVehicle());
        }
    }
}
