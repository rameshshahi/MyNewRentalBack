namespace NewRentalApi.DTOs
{
    public class VerifyPaymentDto
    {
        public int BillId { get; set; }

        public string PaymentGateway { get; set; }

        public string TransactionUuid { get; set; }

        public string Pidx { get; set; }

        public string RefId { get; set; }
    }
}
