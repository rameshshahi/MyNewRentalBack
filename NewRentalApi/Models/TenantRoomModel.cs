using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantRoomModel
    {
        [Key]
        public int TenantRoomId { get; set; }

        public int TenantId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual TenantModel Tenant { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual RoomModel Room { get; set; }

        public DateTime RentStartDate { get; set; }

        public DateTime? RentEndDate { get; set; }

        public decimal MonthlyRent { get; set; }

        public bool IsActive { get; set; }
    }
}
