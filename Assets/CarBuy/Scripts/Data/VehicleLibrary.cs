using System.Collections.Generic;
using System.Linq;
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
            return m_Vehicles.FirstOrDefault(v => v.Id == id);
        }
    }
}
