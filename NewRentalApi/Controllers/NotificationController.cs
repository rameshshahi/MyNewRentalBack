using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewRentalApi.Data;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin.Auth.Multitenancy;
using NewRentalApi.Services;
using NewRentalApi.DTOs;

namespace NewRentalApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/notification/Owner/1
        [HttpGet("{userType}/{userId}")]
        public async Task<IActionResult> GetNotifications(string userType, int userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userType, userId);
            return Ok(notifications);
        }

        // PUT: api/notification/read/5
        [HttpPut("read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new
            {
                Success = true,
                Message = "Notification marked as read."
            });
        }

        // POST: api/notification/send-owner
        [HttpPost("send-owner")]
        public async Task<IActionResult> SendToOwner([FromBody] OwnerNotificationRequest request)
        {
            await _notificationService.SendToOwnerAsync(
                request.OwnerId,
                request.Title,
                request.Message);

            return Ok(new
            {
                Success = true,
                Message = "Notification sent to owner."
            });
        }

        // POST: api/notification/send-tenant
        [HttpPost("send-tenant")]
        public async Task<IActionResult> SendToTenant([FromBody] TenantNotificationRequest request)
        {
            await _notificationService.SendToTenantAsync(
                request.TenantId,
                request.Title,
                request.Message);

            return Ok(new
            {
                Success = true,
                Message = "Notification sent to tenant."
            });
        }

        [HttpPost("save-device-token")]
        public async Task<IActionResult> SaveDeviceToken(DeviceTokenRequest request)
        {
            await _notificationService.SaveDeviceTokenAsync(request);

            return Ok(new
            {
                Success = true,
                Message = "Device token saved successfully."
            });
        }
    }
}
