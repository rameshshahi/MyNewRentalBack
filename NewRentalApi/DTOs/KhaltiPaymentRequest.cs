namespace NewRentalApi.DTOs
{
    public class KhaltiPaymentRequest
    {
        public string PublicKey { get; set; }
        public string ProductIdentity { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }

    }
}
