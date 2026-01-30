using CarBuy.Data;

namespace CarBuy.Services
{
    public interface ITransactionService
    {
        void PurchaseVehicle(string vehicleId, int colorIndex);
        void SellVehicle(string vehicleId);
        event TransactionCompletedHandler PurchaseCompleted;
        event TransactionCompletedHandler SellCompleted;
    }

    public delegate void TransactionCompletedHandler(VehicleTransaction transaction);
}
