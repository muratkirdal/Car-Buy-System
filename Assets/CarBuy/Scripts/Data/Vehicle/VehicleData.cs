using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "CarBuy/Vehicle Data")]
    public class VehicleData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string m_Id;
        [SerializeField] private string m_DisplayName;
        [SerializeField] private VehicleClass m_Class;

        [Header("Economics")]
        [SerializeField] private int m_Price;
        [SerializeField] private int m_SalePrice;

        [Header("Performance Stats")]
        [SerializeField] private VehicleStats m_Stats;

        [Header("Visuals")]
        [SerializeField] private VehicleColorOption[] m_Colors;
        [SerializeField] private VehicleModelInfo m_ModelInfo;

        public string Id => m_Id;
        public string DisplayName => m_DisplayName;
        public VehicleClass Class => m_Class;
        public int Price => m_Price;
        public int SalePrice => m_SalePrice;
        public VehicleStats Stats => m_Stats;
        public VehicleColorOption[] Colors => m_Colors;
        public VehicleModelInfo ModelInfo => m_ModelInfo;
    }
}
