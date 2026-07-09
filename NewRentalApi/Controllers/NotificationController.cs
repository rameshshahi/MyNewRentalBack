using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewRentalApi.Data;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin.Auth.Multitenancy;

namespace NewRentalApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public NotificationController(
            RentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("{tenantId}")]
        public async Task<IActionResult> Get(int tenantId)
        {
            var notifications = await _context.tblNotification
                .Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(notifications);
        }
        [HttpGet("Owner/{ownerId}")]
        public async Task<IActionResult> GetForOwner(int ownerId)
        {
            var notifications = await _context.tblNotification
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpPut("Read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification =
                await _context.tblNotification
                    .FindAsync(id);

            if (notification == null)
                return NotFound();

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Unread/{tenantId}")]
        public async Task<IActionResult> UnreadCount(
            int tenantId)
        {
            var count =
                await _context.tblNotification
                    .CountAsync(x =>
                        x.TenantId == tenantId &&
                        !x.IsRead);

            return Ok(count);
        }

      

        [HttpGet("Owner/Unread/{ownerId}")]
        public async Task<IActionResult> UnreadCountForOwner(int ownerId)
        {
            var count =
                await _context.tblNotification
                    .CountAsync(x => x.OwnerId == ownerId && !x.IsRead);

            return Ok(count);
        }
    }
}
