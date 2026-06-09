using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    
        public class FlatModel
        {
            [Key]
            public int FlatId { get; set; }

            public int FloorId { get; set; }

            [ForeignKey(nameof(FloorId))]
            public virtual FloorModel Floor { get; set; }

            public string FlatNo { get; set; }

            public string FlatName { get; set; }

            public decimal FlatRent { get; set; }

            public bool IsOccupied { get; set; }

            public virtual ICollection<RoomModel> Rooms { get; set; }
        }
    
}
