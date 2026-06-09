using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class MaintenanceModel
    {
        [Key]
        public int MaintenanceId { get; set; }

        public int HouseId { get; set; }

        [ForeignKey(nameof(HouseId))]
        public virtual HouseModel House { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public DateTime MaintenanceDate { get; set; }
    }
}
