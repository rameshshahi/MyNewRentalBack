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
    public class FloorController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public FloorController(RentalDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(FloorDto dto)
        {
            var model = new FloorModel
            {
                HouseId = dto.HouseId,
                FloorName = dto.FloorName,
                FloorNumber = dto.FloorNumber,
                IsActive = true
            };

            await _context.tblFloor.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, FloorDto dto)
        {
            var floor = await _context.tblFloor.FindAsync(id);

            if (floor == null)
                return NotFound();

            floor.FloorName = dto.FloorName;
            floor.FloorNumber = dto.FloorNumber;
            // optionally update HouseId if you allow it:
            // floor.HouseId = dto.HouseId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("List/{houseId}")]
        public async Task<IActionResult> List(int houseId)
        {
            var floors = await _context.tblFloor
                .Where(x => x.HouseId == houseId)
                .ToListAsync();

            return Ok(floors);
        }
    }
}
