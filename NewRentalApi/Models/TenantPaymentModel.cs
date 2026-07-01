using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NewRentalApi.Models
{
    public class TenantPaymentModel
    {
        [Key]
        public int PaymentId { get; set; }

        public int BillId { get; set; }

        public int TenantId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentGateway { get; set; }   // Khalti / eSewa

        public string TransactionId { get; set; }

        public string? Pidx { get; set; }             // Khalti only

        public string? RefId { get; set; }            // eSewa only

        public string Status { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Remarks { get; set; }
    }
}
