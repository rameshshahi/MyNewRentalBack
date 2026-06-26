namespace NewRentalApi.DTOs
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public int BillId { get; set; }
    }
}
