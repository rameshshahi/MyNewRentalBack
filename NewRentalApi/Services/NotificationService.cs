namespace NewRentalApi.Services
{
    using Microsoft.EntityFrameworkCore;
    using NewRentalApi.Data;
    using NewRentalApi.Models;

    public class NotificationService : INotificationService
    {
        private readonly RentalDbContext _context;
        private readonly IFirebaseService _firebaseService;
        private readonly MasterDbContext _masterContext;
        public NotificationService(
            RentalDbContext context,
            IFirebaseService firebaseService,MasterDbContext masterDbContext)
        {
            _context = context;
            _firebaseService = firebaseService;
            _masterContext = masterDbContext;
        }

        public async Task SendToOwnerAsync(
            int ownerId,
            string title,
            string message)
        {
            var owner = await _masterContext.tblOwner
                .FirstOrDefaultAsync(x => x.OwnerId == ownerId);

            if (owner == null)
                return;

            var notification = new NotificationModel
            {
                UserType = "Owner",
                UserId = ownerId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.tblNotification.Add(notification);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(owner.DeviceToken))
            {
                await _firebaseService.SendNotification(
                    owner.DeviceToken,
                    title,
                    message);
            }
        }

        public async Task SendToTenantAsync(
            int tenantId,
            string title,
            string message)
        {
            var tenant = await _context.tblTenant
                .FirstOrDefaultAsync(x => x.TenantId == tenantId);

            if (tenant == null)
                return;

            var notification = new NotificationModel
            {
                UserType = "Tenant",
                UserId = tenantId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.tblNotification.Add(notification);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(tenant.DeviceToken))
            {
                await _firebaseService.SendNotification(
                    tenant.DeviceToken,
                    title,
                    message);
            }
        }

        public async Task<List<NotificationModel>> GetNotificationsAsync(
            string userType,
            int userId)
        {
            return await _context.tblNotification
                .Where(x => x.UserType == userType &&
                            x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.tblNotification
                .FindAsync(notificationId);

            if (notification == null)
                return;

            notification.IsRead = true;

            await _context.SaveChangesAsync();
        }
    }
}
