using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class HouseModel
    {
        [Key]
        public int HouseId { get; set; }

        public string HouseNo { get; set; }

        public string HouseName { get; set; }

        public string HouseAddress { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<FloorModel> Floors { get; set; }
    }
}
