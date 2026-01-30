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
        public event PurchaseFailedHandler PurchaseFailed;

        public TransactionResult PurchaseVehicle(string vehicleId, int colorIndex)
        {
            var vehicle = m_VehicleService.GetVehicle(vehicleId);
            var transaction = new PurchaseTransaction(vehicleId, colorIndex, vehicle.Price);

            if (m_VehicleService.IsVehicleOwned(vehicleId))
            {
                PurchaseFailed?.Invoke(transaction, "Vehicle is already owned.");
                return TransactionResult.AlreadyOwned;
            }

            if (!m_CurrencyService.TryDeduct(vehicle.Price))
            {
                PurchaseFailed?.Invoke(transaction, "Insufficient funds.");
                return TransactionResult.InsufficientFunds;
            }

            m_PlayerData.AddVehicle(new OwnedVehicle(vehicleId, colorIndex));

            PurchaseCompleted?.Invoke(transaction);

            return TransactionResult.Success;
        }
    }
}
