using CarBuy.Data;

namespace CarBuy.Services
{
    public interface ITransactionService
    {
        TransactionResult PurchaseVehicle(string vehicleId, int colorIndex);
        event PurchaseCompletedHandler PurchaseCompleted;
        event PurchaseFailedHandler PurchaseFailed;
    }

    public delegate void PurchaseCompletedHandler(PurchaseTransaction transaction);
    public delegate void PurchaseFailedHandler(PurchaseTransaction transaction, string errorMessage);
}
