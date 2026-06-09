using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class RentPaymentModel
    {
        [Key]
        public int PaymentId { get; set; }

        public int TenantId { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual TenantModel Tenant { get; set; }

        public string PaymentMonth { get; set; }

        public decimal RentAmount { get; set; }

        public decimal FineAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }

        public string Remarks { get; set; }
    }
}
