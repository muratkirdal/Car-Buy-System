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

            if (vehicle == null)
            {
                return TransactionResult.ServerError;
            }

            var transaction = new PurchaseTransaction
            {
                VehicleId = vehicleId,
                ColorIndex = colorIndex,
                Price = vehicle.Price
            };

            if (m_VehicleService.IsVehicleOwned(vehicleId))
            {
                PurchaseFailed?.Invoke(transaction, "Vehicle is already owned.");
                return TransactionResult.AlreadyOwned;
            }

            if (!m_CurrencyService.CanAfford(vehicle.Price))
            {
                PurchaseFailed?.Invoke(transaction, "Insufficient funds.");
                return TransactionResult.InsufficientFunds;
            }

            if (!m_CurrencyService.TryDeduct(vehicle.Price))
            {
                PurchaseFailed?.Invoke(transaction, "Failed to deduct funds.");
                return TransactionResult.ServerError;
            }

            m_PlayerData.AddVehicle(new OwnedVehicle
            {
                VehicleId = vehicleId
            });

            PurchaseCompleted?.Invoke(transaction);

            return TransactionResult.Success;
        }
    }
}
