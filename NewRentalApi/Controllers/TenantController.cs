using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRentalApi.Data;
using NewRentalApi.DTOs;
using NewRentalApi.Models;

namespace NewRentalApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly RentalDbContext _context;
        private readonly MasterDbContext _masterContext;
        public TenantController(RentalDbContext context, MasterDbContext masterContext  )
        {
            _context = context;
            _masterContext = masterContext;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(TenantModel model)
        {
            try
            {
                var existingTenant = await _context.tblTenant
                    .FirstOrDefaultAsync(x => x.PhoneNo == model.PhoneNo);

                if (existingTenant != null)
                    return BadRequest("Phone number already exists.");

                model.CreatedDate = DateTime.Now;
                model.IsActive = true;

                await _context.tblTenant.AddAsync(model);
                await _context.SaveChangesAsync();

                var databaseName =
                    User.FindFirst("DatabaseName")?.Value;

                var ownerId =
                    Convert.ToInt32(
                        User.FindFirst("OwnerId")?.Value);

                await _masterContext.tblTenantLogin.AddAsync(
                    new TenantLoginModel
                    {
                        TenantId = model.TenantId,
                        FullName = model.FullName,
                        PhoneNo = model.PhoneNo,
                        OwnerId = ownerId,
                        DatabaseName = databaseName,
                        IsActive = true
                    });

                await _masterContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    TenantId = model.TenantId,
                    Message = "Tenant Added Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, TenantModel model)
        {
            try
            {
                var tenant = await _context.tblTenant
                    .FirstOrDefaultAsync(x => x.TenantId == id);

                if (tenant == null)
                    return NotFound("Tenant not found");

                tenant.FullName = model.FullName;
                tenant.PhoneNo = model.PhoneNo;
                tenant.CitizenshipNo = model.CitizenshipNo;
                tenant.PermanentAddress = model.PermanentAddress;

                var login = await _masterContext.tblTenantLogin
                    .FirstOrDefaultAsync(x => x.TenantId == id);

                if (login != null)
                {
                    login.FullName = model.FullName;
                    login.PhoneNo = model.PhoneNo;
                }

                await _context.SaveChangesAsync();
                await _masterContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Tenant Updated Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tenant = await _context.tblTenant
                    .FirstOrDefaultAsync(x => x.TenantId == id);

                if (tenant == null)
                    return NotFound();

                // 1. Get all active tenant-room records
                var tenantRooms = await _context.tblTenantRoom
                    .Where(x => x.TenantId == id && x.IsActive == true)
                    .ToListAsync();

                // 2. UPDATE tenant-room + room data HERE
                foreach (var tr in tenantRooms)
                {
                    tr.RentEndDate = DateTime.Now;
                    tr.IsActive = false;

                    var room = await _context.tblRoom
                        .FirstOrDefaultAsync(r => r.RoomId == tr.RoomId);

                    if (room != null)
                    {
                        room.IsOccupied = false;   // available
                        //room.RoomId = null;
                    }
                }

                // 3. Soft delete tenant
                tenant.IsActive = false;

                var login = await _masterContext.tblTenantLogin.FirstOrDefaultAsync(x => x.TenantId == id);

                if (login != null)
                {
                    login.IsActive = false;
                }

                // 4. Save ALL changes at once
                await _context.SaveChangesAsync();
                await _masterContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Tenant Deleted Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            return Ok(await _context.tblTenant
                .OrderBy(x => x.FullName)
                .ToListAsync());
        }

        [HttpGet("Active")]
        public async Task<IActionResult> Active()
        {
            return Ok(await _context.tblTenant
                .Where(x => x.IsActive)
                .OrderBy(x => x.FullName)
                .ToListAsync());
        }

        [HttpGet("Detail/{tenantId}")]
        public async Task<IActionResult> Detail(int tenantId)
        {
            var tenant = await _context.tblTenant
                .Include(x => x.TenantRooms)
                .ThenInclude(x => x.Room)
                .FirstOrDefaultAsync(x => x.TenantId == tenantId);

            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        [HttpPost("AssignRoom")]
        public async Task<IActionResult> AssignRoom(
            AssignRoomDto dto)
        {
            try
            {
                var tenant = await _context.tblTenant
                    .FirstOrDefaultAsync(x =>
                        x.TenantId == dto.TenantId);

                if (tenant == null)
                    return NotFound("Tenant not found");

                foreach (var roomId in dto.RoomIds)
                {
                    var room = await _context.tblRoom
                        .FirstOrDefaultAsync(x =>
                            x.RoomId == roomId);

                    if (room == null)
                        continue;

                    if (room.IsOccupied)
                    {
                        return BadRequest(
                            $"Room {room.RoomNo} is already occupied.");
                    }

                    room.IsOccupied = true;

                    await _context.tblTenantRoom.AddAsync(
                        new TenantRoomModel
                        {
                            TenantId = dto.TenantId,
                            RoomId = roomId,
                            RentStartDate = dto.RentStartDate,
                            MonthlyRent = dto.MonthlyRent,
                            IsActive = true
                        });
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Room Assigned Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("VacateRoom/{tenantRoomId}")]
        public async Task<IActionResult> VacateRoom(
            int tenantRoomId)
        {
            try
            {
                var tenantRoom =
                    await _context.tblTenantRoom
                        .FirstOrDefaultAsync(x =>
                            x.TenantRoomId == tenantRoomId);

                if (tenantRoom == null)
                    return NotFound();

                tenantRoom.IsActive = false;
                tenantRoom.RentEndDate = DateTime.Now;

                var room = await _context.tblRoom
                    .FirstOrDefaultAsync(x =>
                        x.RoomId == tenantRoom.RoomId);

                var otherActiveAssignment =
                    await _context.tblTenantRoom
                        .AnyAsync(x =>
                            x.RoomId == tenantRoom.RoomId &&
                            x.IsActive &&
                            x.TenantRoomId != tenantRoomId);

                if (!otherActiveAssignment && room != null)
                {
                    room.IsOccupied = false;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Room Vacated Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VacantRooms")]
        public async Task<IActionResult> VacantRooms()
        {
            var rooms = await _context.tblRoom
                .Where(x => !x.IsOccupied)
                .OrderBy(x => x.RoomNo)
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("RoomHistory/{tenantId}")]
        public async Task<IActionResult> RoomHistory(
            int tenantId)
        {
            var history =
                await _context.tblTenantRoom
                    .Include(x => x.Room)
                    .Where(x => x.TenantId == tenantId)
                    .OrderByDescending(x => x.RentStartDate)
                    .ToListAsync();

            return Ok(history);
        }
    }
}