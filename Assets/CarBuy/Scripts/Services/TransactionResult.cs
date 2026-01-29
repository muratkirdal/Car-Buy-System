namespace CarBuy.Services
{
    /// <summary>
    /// Represents the possible outcomes of a vehicle purchase transaction.
    /// </summary>
    public enum TransactionResult
    {
        Success,
        InsufficientFunds,
        AlreadyOwned,
        ServerError
    }
}
