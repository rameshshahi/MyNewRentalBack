using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantLoginModel
    {
        [Key]
        public int TenantLoginId { get; set; }

        public int TenantId { get; set; }

        public string FullName { get; set; }

        public string PhoneNo { get; set; }

        public int OwnerId { get; set; }

        public string DatabaseName { get; set; }

        public string? DeviceToken { get; set; }
        public bool IsActive { get; set; }
    }
}
