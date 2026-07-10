using System.ComponentModel.DataAnnotations;
namespace NewRentalApi.Models
{


    public class NotificationModel
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserType { get; set; } = string.Empty; // Owner or Tenant

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string NotificationType { get; set; } = string.Empty;
        // BillGenerated, PaymentReceived, General, Reminder

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
