using System.ComponentModel.DataAnnotations;
namespace NewRentalApi.Models
{


    public class NotificationModel
    {
        [Key]
        public int NotificationId { get; set; }
        public int? TenantId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual TenantModel Tenant { get; set; }

        // Add the missing OwnerId property to fix the error
        public int? OwnerId { get; set; }

        public virtual OwnerModel Owner { get; set; }
    }
}
