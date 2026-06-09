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
    public class RoomController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public RoomController(RentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("List/{flatId}")]
        public async Task<IActionResult> GetRooms(int flatId)
        {
            var rooms = await _context.tblRoom
                .Where(r => r.FlatId == flatId)
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.tblRoom.FindAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = new RoomModel
            {
                FlatId = dto.FlatId,
                RoomNo = dto.RoomNo,
                RoomName = dto.RoomName,
                MonthlyRent = dto.MonthlyRent,
                IsOccupied = dto.IsOccupied
            };

            _context.tblRoom.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, room);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.tblRoom.FindAsync(id);
            if (existing == null) return NotFound();

            existing.FlatId = dto.FlatId;
            existing.RoomNo = dto.RoomNo;
            existing.RoomName = dto.RoomName;
            existing.MonthlyRent = dto.MonthlyRent;
            existing.IsOccupied = dto.IsOccupied;

            _context.tblRoom.Update(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var existing = await _context.tblRoom.FindAsync(id);
            if (existing == null) return NotFound();

            _context.tblRoom.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Vacant")]
        public async Task<IActionResult> GetVacantRooms()
        {
            var rooms = await _context.tblRoom
                .Where(r => !r.IsOccupied)
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("Occupied")]
        public async Task<IActionResult> GetOccupiedRooms()
        {
            var rooms = await _context.tblRoom
                .Where(r => r.IsOccupied)
                .ToListAsync();

            return Ok(rooms);
        }
    }

}
