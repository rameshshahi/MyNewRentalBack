namespace NewRentalApi.DTOs
{
    public class OwnerNotificationRequest
    {
        public int OwnerId { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
