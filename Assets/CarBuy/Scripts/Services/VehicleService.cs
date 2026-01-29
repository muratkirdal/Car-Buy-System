using CarBuy.Data;

namespace CarBuy.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleLibrary m_Library;
        private readonly PlayerShopData m_PlayerData;

        public VehicleService(VehicleLibrary library, PlayerShopData playerData)
        {
            m_Library = library;
            m_PlayerData = playerData;
        }

        public VehicleData GetVehicle(string id)
        {
            return m_Library.GetById(id);
        }

        public bool IsVehicleOwned(string id)
        {
            return m_PlayerData.OwnsVehicle(id);
        }
    }
}
