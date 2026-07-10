using NewRentalApi.Models;

namespace NewRentalApi.Services
{
    public interface INotificationService
    {
        Task SendToOwnerAsync(
            int ownerId,
            string title,
            string message);

        Task SendToTenantAsync(
            int tenantId,
            string title,
            string message);

        Task<List<NotificationModel>> GetNotificationsAsync(
            string userType,
            int userId);

        Task MarkAsReadAsync(int notificationId);
    }
}
