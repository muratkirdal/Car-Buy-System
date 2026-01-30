using UnityEngine;
using TMPro;
using CarBuy.Services;

namespace CarBuy.UI.HUD
{
    public class CurrencyHudView : MonoBehaviour
    {
        private const string k_CurrencyFormat = "${0:N0}";

        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_BalanceText;

        private ICurrencyService m_CurrencyService;

        private void OnDestroy()
        {
            UnsubscribeFromService();
        }

        public void Initialize(ICurrencyService currencyService)
        {
            m_CurrencyService = currencyService;
            m_CurrencyService.BalanceChanged += OnBalanceChanged;

            SetBalance(m_CurrencyService.CurrentBalance);
        }

        private void OnBalanceChanged(int newBalance)
        {
            SetBalance(newBalance);
        }

        private void SetBalance(int balance)
        {
            m_BalanceText.text = string.Format(k_CurrencyFormat, balance);
        }

        private void UnsubscribeFromService()
        {
            m_CurrencyService.BalanceChanged -= OnBalanceChanged;
        }
    }
}
