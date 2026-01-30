using CarBuy.Data;

namespace CarBuy.Services
{
    public interface ITransactionService
    {
        void PurchaseVehicle(string vehicleId, int colorIndex);
        event PurchaseCompletedHandler PurchaseCompleted;
    }

    public delegate void PurchaseCompletedHandler(PurchaseTransaction transaction);
}
