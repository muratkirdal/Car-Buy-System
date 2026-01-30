namespace CarBuy.Services
{
    public interface ICurrencyService
    {
        int CurrentBalance { get; }
        bool TryDeduct(int amount);
        void Add(int amount);
        event BalanceChangedHandler BalanceChanged;
    }

    public delegate void BalanceChangedHandler(int newBalance);
}
