namespace CarBuy.Data
{
    public class VehicleTransaction
    {
        private readonly string m_VehicleId;

        public VehicleTransaction(string vehicleId)
        {
            m_VehicleId = vehicleId;
        }

        public string VehicleId => m_VehicleId;
    }
}