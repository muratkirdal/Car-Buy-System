using CarBuy.Data;

namespace CarBuy.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly PlayerShopData m_PlayerData;

        public CurrencyService(PlayerShopData playerData)
        {
            m_PlayerData = playerData;
        }

        public int CurrentBalance => m_PlayerData.Balance;

        public event BalanceChangedHandler BalanceChanged;

        public bool CanAfford(int amount)
        {
            return m_PlayerData.Balance >= amount;
        }

        public bool TryDeduct(int amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            m_PlayerData.DecreaseBalance(amount);
            BalanceChanged?.Invoke(m_PlayerData.Balance);
            return true;
        }

        public void Add(int amount)
        {
            m_PlayerData.IncreaseBalance(amount);
            BalanceChanged?.Invoke(m_PlayerData.Balance);
        }
    }
}
