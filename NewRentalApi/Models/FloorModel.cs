using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
   
        public class FloorModel
        {
            [Key]
            public int FloorId { get; set; }

            public int HouseId { get; set; }

            [ForeignKey(nameof(HouseId))]
            public virtual HouseModel? House { get; set; }

            public string FloorName { get; set; }

            public int FloorNumber { get; set; }

            public bool IsActive { get; set; } = true;

            public virtual ICollection<FlatModel>? Flats { get; set; }
        }
    
}
