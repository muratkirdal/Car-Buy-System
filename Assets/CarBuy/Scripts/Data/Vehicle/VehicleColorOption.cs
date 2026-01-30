using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct VehicleColorOption
    {
        [SerializeField] private Color m_Color;

        public Color Color => m_Color;
    }
}
