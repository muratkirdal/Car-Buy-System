using CarBuy.Data;

namespace CarBuy.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IVehicleService m_VehicleService;
        private readonly ICurrencyService m_CurrencyService;
        private readonly PlayerShopData m_PlayerData;

        public TransactionService(
            IVehicleService vehicleService,
            ICurrencyService currencyService,
            PlayerShopData playerData)
        {
            m_VehicleService = vehicleService;
            m_CurrencyService = currencyService;
            m_PlayerData = playerData;
        }

        public event TransactionCompletedHandler PurchaseCompleted;
        public event TransactionCompletedHandler SellCompleted;

        public void PurchaseVehicle(string vehicleId, int colorIndex)
        {
            var vehicle = m_VehicleService.GetVehicle(vehicleId);

            if (m_VehicleService.IsVehicleOwned(vehicleId)) return;
            if (!m_CurrencyService.TryDeduct(vehicle.Price)) return;

            m_PlayerData.AddVehicle(new OwnedVehicle(vehicleId, colorIndex));

            var transaction = new VehicleTransaction(vehicleId);
            PurchaseCompleted?.Invoke(transaction);
        }

        public void SellVehicle(string vehicleId)
        {
            var vehicle = m_VehicleService.GetVehicle(vehicleId);

            m_PlayerData.RemoveVehicle(vehicleId);
            m_CurrencyService.Add(vehicle.SalePrice);

            var transaction = new VehicleTransaction(vehicleId);
            SellCompleted?.Invoke(transaction);
        }
    }
}
