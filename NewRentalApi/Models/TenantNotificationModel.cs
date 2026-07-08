using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantNotificationModel:NotificationModel
    {
        [Key]
        public int TenantNotificationId { get; set; }
        public int TenantId { get; set; }
        public virtual TenantModel Tenant { get; set; }
    }
}
