namespace NewRentalApi.Services
{
    using Microsoft.EntityFrameworkCore;
    using NewRentalApi.Data;
    using NewRentalApi.Models;

    public class NotificationService : INotificationService
    {
        private readonly RentalDbContext _context;
        private readonly MasterDbContext _masterContext;
        private readonly IFirebaseService _firebase;

        public NotificationService(
            RentalDbContext context,
            MasterDbContext masterContext,
            IFirebaseService firebase)
        {
            _context = context;
            _masterContext = masterContext;
            _firebase = firebase;
        }

        public async Task SaveAndSendNotification(
            int? tenantId,
            int? ownerId,
            string title,
            string message,
            string type)
        {
            var notification =
                new NotificationModel
                {
                    TenantId = tenantId,
                    OwnerId = ownerId,
                    Title = title,
                    Message = message,
                    NotificationType = type,
                    IsRead = false,
                    CreatedDate = DateTime.Now
                };

            await _context.tblNotification.AddAsync(notification);

            await _context.SaveChangesAsync();

            var login =
                await _masterContext.tblTenantLogin
                    .FirstOrDefaultAsync(x =>
                        x.TenantId == tenantId &&
                        x.IsActive);

            if (login != null &&
                !string.IsNullOrWhiteSpace(login.DeviceToken))
            {
                await _firebase.SendNotification(
                    login.DeviceToken,
                    title,
                    message);
            }
        }
    }
}
