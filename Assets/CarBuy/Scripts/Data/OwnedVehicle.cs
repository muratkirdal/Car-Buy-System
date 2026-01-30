using System;

namespace CarBuy.Data
{
    [Serializable]
    public readonly struct OwnedVehicle
    {
        private readonly string m_VehicleId;
        private readonly int m_ColorIndex;

        public OwnedVehicle(string vehicleId, int colorIndex)
        {
            m_VehicleId = vehicleId;
            m_ColorIndex = colorIndex;
        }

        public string VehicleId => m_VehicleId;
        // Use for garage system
        public int ColorIndex => m_ColorIndex;
    }
}
