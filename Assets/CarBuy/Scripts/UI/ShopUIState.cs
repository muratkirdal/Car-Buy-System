using System;
using CarBuy.Data;

namespace CarBuy.UI
{
    [Serializable]
    public class ShopUIState
    {
        public int CurrentVehicleIndex;
        public int SelectedColorIndex;
        public VehicleData CurrentVehicle;
    }
}
