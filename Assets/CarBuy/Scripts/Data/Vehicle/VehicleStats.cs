using System;
using UnityEngine;

namespace CarBuy.Data
{
    [Serializable]
    public struct VehicleStats
    {
        [SerializeField, Range(0, 100)] private int m_Speed;
        [SerializeField, Range(0, 100)] private int m_Acceleration;
        [SerializeField, Range(0, 100)] private int m_Handling;

        public int Speed => m_Speed;
        public int Acceleration => m_Acceleration;
        public int Handling => m_Handling;
    }
}
