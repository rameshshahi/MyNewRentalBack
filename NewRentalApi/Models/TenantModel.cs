using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantModel
    {
        [Key]
        public int TenantId { get; set; }

        public string FullName { get; set; }

        public string PhoneNo { get; set; }

        public string CitizenshipNo { get; set; }

        public string PermanentAddress { get; set; }

        public string TemporaryAddress { get; set; }

        public string Occupation { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactNo { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<TenantRoomModel> TenantRooms { get; set; }
    }
}
