namespace CarBuy.Services
{
    public interface ICurrencyService
    {
        int CurrentBalance { get; }
        bool CanAfford(int amount);
        bool TryDeduct(int amount);
        void Add(int amount);
        event BalanceChangedHandler BalanceChanged;
    }

    public delegate void BalanceChangedHandler(int newBalance);
}
