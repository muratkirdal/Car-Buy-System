using System;
using System.Collections.Generic;

namespace CarBuy.Data
{
    [Serializable]
    public class PlayerShopData
    {
        private int m_Balance;
        private List<OwnedVehicle> m_OwnedVehicles = new();

        public PlayerShopData(int balance)
        {
            m_Balance = balance;
        }

        public int Balance => m_Balance;

        public void IncreaseBalance(int amount)
        {
            m_Balance += amount;
        }

        public void DecreaseBalance(int amount)
        {
            m_Balance -= amount;
        }

        public void AddVehicle(OwnedVehicle vehicle)
        {
            m_OwnedVehicles.Add(vehicle);
        }

        public bool OwnsVehicle(string vehicleId)
        {
            foreach (OwnedVehicle vehicle in m_OwnedVehicles)
            {
                if (vehicle.VehicleId == vehicleId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
