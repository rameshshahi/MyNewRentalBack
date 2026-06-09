using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRentalApi.Data;

namespace NewRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly RentalDbContext _context;
        public DashboardController(RentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("Summary")]
        public async Task<IActionResult> Summary()
        {
            var totalHouse =
                await _context.tblHouse.CountAsync(x=>x.IsDeleted==false);

            var totalFloor =
                await _context.tblFloor.CountAsync();

            var totalFlat =
                await _context.tblFlat.CountAsync();

            var totalRoom =
                await _context.tblRoom.CountAsync();

            var occupiedRoom =
                await _context.tblRoom
                    .CountAsync(x => x.IsOccupied);

            var vacantRoom =
                await _context.tblRoom
                    .CountAsync(x => !x.IsOccupied);

            var totalTenant =
                await _context.tblTenant.CountAsync();

            var income =
                await _context.tblRentPayment
                    .SumAsync(x => x.PaidAmount);

            var expense =
                await _context.tblExpense
                    .SumAsync(x => x.Amount);

            return Ok(new
            {
                totalHouse,
                totalFloor,
                totalFlat,
                totalRoom,
                occupiedRoom,
                vacantRoom,
                totalTenant,
                income,
                expense
            });
        }
    }
    }
