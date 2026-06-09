    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomId { get; set; }

        public int FlatId { get; set; }

        [ForeignKey(nameof(FlatId))]
        public virtual FlatModel Flat { get; set; }

        public string RoomNo { get; set; }

        public string RoomName { get; set; }

        public decimal MonthlyRent { get; set; }

        public bool IsOccupied { get; set; }
    }
}
