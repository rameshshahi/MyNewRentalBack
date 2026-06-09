using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class TenantDocumentModel
    {
        [Key]
        public int DocumentId { get; set; }

        public int TenantId { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual TenantModel Tenant { get; set; }

        public string DocumentName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}
