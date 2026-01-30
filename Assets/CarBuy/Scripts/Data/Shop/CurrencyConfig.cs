using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "CurrencyConfig", menuName = "CarBuy/Configs/Currency")]
    public class CurrencyConfig : ScriptableObject
    {
        [SerializeField] private int m_StartingBalance = 50000;

        public int StartingBalance => m_StartingBalance;
    }
}
