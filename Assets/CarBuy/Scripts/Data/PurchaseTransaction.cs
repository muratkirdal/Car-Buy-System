namespace CarBuy.Data
{
    public class PurchaseTransaction
    {
        private readonly string m_VehicleId;

        public PurchaseTransaction(string vehicleId)
        {
            m_VehicleId = vehicleId;
        }

        public string VehicleId => m_VehicleId;
    }
}