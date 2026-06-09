using System.ComponentModel.DataAnnotations;

namespace NewRentalApi.Models
{
    public class ExpenseModel
    {
        [Key]
        public int ExpenseId { get; set; }

        public string ExpenseTitle { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string Remarks { get; set; }
    }
}
