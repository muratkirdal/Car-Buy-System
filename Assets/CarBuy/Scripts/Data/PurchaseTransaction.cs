namespace CarBuy.Data
{
    public class PurchaseTransaction
    {
        private readonly string m_VehicleId;
        private readonly int m_ColorIndex;
        private readonly int m_Price;

        public PurchaseTransaction(string vehicleId, int colorIndex, int price)
        {
            m_VehicleId = vehicleId;
            m_ColorIndex = colorIndex;
            m_Price = price;
        }

        public string VehicleId => m_VehicleId;
        public int ColorIndex => m_ColorIndex;
        public int Price => m_Price;
    }
}