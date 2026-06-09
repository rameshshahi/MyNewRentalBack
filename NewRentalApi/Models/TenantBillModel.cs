using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantBillModel
    {
        [Key]
        public int BillId { get; set; }

        public int TenantId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal RentAmount { get; set; }

        public decimal ElectricityCharge { get; set; }

        public decimal WaterCharge { get; set; }

        public decimal GarbageCharge { get; set; }

        public decimal InternetCharge { get; set; }

        public decimal PreviousDue { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingDue { get; set; }

        public bool IsPaid { get; set; }

        public DateTime BillDate { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual TenantModel Tenant { get; set; }
    }
}
