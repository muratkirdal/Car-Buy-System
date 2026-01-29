using CarBuy.Data;

namespace CarBuy.Services
{
    public interface IVehicleService
    {
        VehicleData GetVehicle(string id);
        bool IsVehicleOwned(string id);
    }
}
