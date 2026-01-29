using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct CurrencySettings
    {
        [SerializeField] private int m_StartingBalance;

        public int StartingBalance => m_StartingBalance;
    }
}