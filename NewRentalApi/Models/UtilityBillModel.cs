using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class UtilityBillModel
    {
        [Key]
        public int UtilityBillId { get; set; }

        public int TenantId { get; set; }

        public string BillMonth { get; set; }

        public decimal ElectricityCharge { get; set; }

        public decimal WaterCharge { get; set; }

        public decimal InternetCharge { get; set; }

        public decimal GarbageCharge { get; set; }

        public decimal OtherCharge { get; set; }

        public bool IsPaid { get; set; }
    }
}
