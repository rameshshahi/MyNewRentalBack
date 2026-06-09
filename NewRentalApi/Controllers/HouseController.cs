using Microsoft.AspNetCore.Mvc;

namespace NewRentalApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NewRentalApi.Data;
    using NewRentalApi.Models;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public HouseController(RentalDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddHouse(HouseModel model)
        {
            try
            {
                await _context.tblHouse.AddAsync(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "House added successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditHouse(int id, HouseModel model)
        {
            try
            {
                var house = await _context.tblHouse
                    .FirstOrDefaultAsync(x => x.HouseId == id);

                if (house == null)
                    return NotFound();

                house.HouseNo = model.HouseNo;
                house.HouseName = model.HouseName;
                house.HouseAddress = model.HouseAddress;
                house.Description = model.Description;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "House updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var house = await _context.tblHouse
                .Include(x => x.Floors)
                .FirstOrDefaultAsync(x => x.HouseId == id);

            if (house == null)
                return NotFound();

            return Ok(house);
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var houses = await _context.tblHouse
        .Where(x => !x.IsDeleted)
        .OrderBy(x => x.HouseName)
        .ToListAsync();

            return Ok(houses);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            var house = await _context.tblHouse
                .FirstOrDefaultAsync(x => x.HouseId == id);

            if (house == null)
                return NotFound();

            house.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "House deleted successfully"
            });
        }
        [HttpGet("HouseSummary/{houseId}")]
        public async Task<IActionResult> HouseSummary(int houseId)
        {
            var floors =
                await _context.tblFloor
                    .Where(x => x.HouseId == houseId)
                    .CountAsync();

            return Ok(new
            {
                HouseId = houseId,
                TotalFloors = floors
            });
        }
    }
}
