using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class OwnerNotificationModel:NotificationModel
    {
        [Key]
        public int OwnerNotificationId { get; set; }
        public int OnwerId { get; set; }
        public virtual OwnerModel Owner { get; set; }
    }
}
