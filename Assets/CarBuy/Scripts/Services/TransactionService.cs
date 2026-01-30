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

        public event PurchaseCompletedHandler PurchaseCompleted;

        public void PurchaseVehicle(string vehicleId, int colorIndex)
        {
            var vehicle = m_VehicleService.GetVehicle(vehicleId);

            if (m_VehicleService.IsVehicleOwned(vehicleId)) return;
            if (!m_CurrencyService.TryDeduct(vehicle.Price)) return;

            m_PlayerData.AddVehicle(new OwnedVehicle(vehicleId, colorIndex));

            var transaction = new PurchaseTransaction(vehicleId, colorIndex, vehicle.Price);
            PurchaseCompleted?.Invoke(transaction);
        }
    }
}
