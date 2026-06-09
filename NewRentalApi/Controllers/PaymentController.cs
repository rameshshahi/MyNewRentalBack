using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRentalApi.Data;
using NewRentalApi.Models;

namespace NewRentalApi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly RentalDbContext _context;
        PaymentController(RentalDbContext context)
        {
            _context = context;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(RentPaymentModel payment)
        {
            payment.PaymentDate = DateTime.Now;

            await _context.tblRentPayment.AddAsync(payment);

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("History/{tenantId}")]
        public async Task<IActionResult> History(int tenantId)
        {
            return Ok(await _context.tblRentPayment
                .Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync());
        }
        [HttpGet("CurrentMonth/{tenantId}")]
        public async Task<IActionResult> CurrentMonth(int tenantId)
        {
            string month =
                DateTime.Now.ToString("yyyy-MM");

            var payment =
                await _context.tblRentPayment
                    .FirstOrDefaultAsync(x =>
                        x.TenantId == tenantId &&
                        x.PaymentMonth == month);

            return Ok(payment);
        }

    }
}
