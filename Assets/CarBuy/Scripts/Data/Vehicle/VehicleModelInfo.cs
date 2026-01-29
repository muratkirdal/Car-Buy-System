using System;
using CarBuy.Vehicle;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct VehicleModelInfo
    {
        [SerializeField] private VehicleDisplayInstance m_Prefab;
        [SerializeField] private Sprite m_Icon;

        public VehicleDisplayInstance PrefabReference => m_Prefab;
        public Sprite IconSprite => m_Icon;
    }
}
