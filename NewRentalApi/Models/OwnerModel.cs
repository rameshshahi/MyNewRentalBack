using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
        public class OwnerModel
        {
            [Key]
            public int OwnerId { get; set; }

            [Required]
            public string OwnerName { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public string PasswordHash { get; set; }

            public string PhoneNo { get; set; }

            public string DatabaseName { get; set; }

            public bool IsActive { get; set; } = true;

            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
    }
}
