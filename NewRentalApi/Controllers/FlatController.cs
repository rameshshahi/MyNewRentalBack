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
    public class FlatController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public FlatController(RentalDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(FlatDto dto)
        {
            var model = new FlatModel
            {
                FloorId = dto.FloorId,
                FlatNo = dto.FlatNo,
                FlatName = dto.FlatName,
                FlatRent = dto.FlatRent,
                IsOccupied = dto.IsOccupied
            };

            await _context.tblFlat.AddAsync(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, FlatDto dto)
        {
            var flat = await _context.tblFlat.FindAsync(id);
            if (flat == null) return NotFound();

            flat.FlatNo = dto.FlatNo;
            flat.FlatName = dto.FlatName;
            flat.FlatRent = dto.FlatRent;
            flat.IsOccupied = dto.IsOccupied;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("List/{floorId}")]
        public async Task<IActionResult> List(int floorId)
        {
            return Ok(await _context.tblFlat
                .Where(x => x.FloorId == floorId)
                .ToListAsync());
        }
        [HttpGet("Detail/{flatId}")]
        public async Task<IActionResult> Detail(int flatId)
        {
            var flat = await _context.tblFlat
                .Include(x => x.Rooms)
                .FirstOrDefaultAsync(x => x.FlatId == flatId);

            return Ok(flat);
        }
        [HttpGet("Occupancy/{flatId}")]
        public async Task<IActionResult> Occupancy(int flatId)
        {
            var rooms = await _context.tblRoom
                .Where(x => x.FlatId == flatId)
                .ToListAsync();

            return Ok(new
            {
                TotalRooms = rooms.Count,
                OccupiedRooms = rooms.Count(x => x.IsOccupied),
                VacantRooms = rooms.Count(x => !x.IsOccupied)
            });
        }
       
    }
}
