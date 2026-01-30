using UnityEngine;
using TMPro;
using CarBuy.Services;

namespace CarBuy.UI
{
    public class CurrencyHudView : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_BalanceText;

        private ICurrencyService m_CurrencyService;

        private void OnDestroy()
        {
            m_CurrencyService.BalanceChanged -= OnBalanceChanged;
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
            m_BalanceText.text = string.Format(StringConst.CurrencyFormat, balance);
        }
    }
}
