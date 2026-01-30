using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "VehicleCatalog", menuName = "CarBuy/Vehicle Catalog")]
    public class VehicleLibrary : ScriptableObject
    {
        [SerializeField] private List<VehicleData> m_Vehicles = new();

        public IReadOnlyList<VehicleData> Vehicles => m_Vehicles;

        public VehicleData GetById(string id)
        {
            foreach (VehicleData vehicle in m_Vehicles)
            {
                if (vehicle.Id == id)
                {
                    return vehicle;
                }
            }

            throw new NullReferenceException();
        }
    }
}
