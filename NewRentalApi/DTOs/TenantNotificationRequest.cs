namespace NewRentalApi.DTOs
{
    public class TenantNotificationRequest
    {
        public int TenantId { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
