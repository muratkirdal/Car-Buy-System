using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct ClassIconEntry
    {
        [SerializeField] private VehicleClass m_Class;
        [SerializeField] private Sprite m_Icon;

        public VehicleClass Class => m_Class;
        public Sprite Icon => m_Icon;
    }
}
