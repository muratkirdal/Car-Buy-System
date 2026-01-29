using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CarBuy.Data
{
    [Serializable]
    public struct VehicleColorOption
    {
        [SerializeField, FormerlySerializedAs("Color")] private Color m_Color;

        public Color Color => m_Color;
    }
}
