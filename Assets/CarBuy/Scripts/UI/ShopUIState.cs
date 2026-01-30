using System;
using CarBuy.Data;

namespace CarBuy.UI
{
    [Serializable]
    public class ShopUIState
    {
        public int CurrentVehicleIndex;
        public int SelectedColorIndex;
        public bool IsPopupOpen;
        public bool IsProcessingPurchase;
        public VehicleData CurrentVehicle;
    }
}
