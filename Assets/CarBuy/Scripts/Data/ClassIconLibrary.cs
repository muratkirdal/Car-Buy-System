using System.Collections.Generic;
using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "ClassIconLibrary", menuName = "CarBuy/Class Icon Library")]
    public class ClassIconLibrary : ScriptableObject
    {
        [SerializeField] private List<ClassIconEntry> m_Icons = new();

        public Sprite GetIcon(VehicleClass vehicleClass)
        {
            foreach (var entry in m_Icons)
            {
                if (entry.Class == vehicleClass)
                {
                    return entry.Icon;
                }
            }

            return null;
        }
    }
}
