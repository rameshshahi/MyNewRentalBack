namespace NewRentalApi.DTOs
{
    public class GenerateBillDto
    {
        public int TenantId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal ElectricityCharge { get; set; }

        public decimal WaterCharge { get; set; }

        public decimal GarbageCharge { get; set; }

        public decimal InternetCharge { get; set; }
    }
}
