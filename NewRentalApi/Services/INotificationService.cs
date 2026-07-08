namespace NewRentalApi.Services
{
    public interface INotificationService
    {
        Task SaveAndSendNotification(
      int? tenantId,
      int? ownerId,
      string title,
      string message,
      string type);
    }
}
