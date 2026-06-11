using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRentalApi.Data;
using NewRentalApi.DTOs;
using NewRentalApi.Models;
using NewRentalApi.Services;

namespace NewRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly RentalDbContext _context;

        public BillController(RentalDbContext context)
        {
            _context = context;
        }




        [HttpPost("GenerateBill")]
        public async Task<IActionResult> GenerateBill(GenerateBillDto dto)
        {
            var tenant = await _context.tblTenant
                .Include(x => x.TenantRooms)
                .FirstOrDefaultAsync(x =>
                    x.TenantId == dto.TenantId);

            if (tenant == null)
                return NotFound();

            decimal monthlyRent =
                tenant.TenantRooms
                    .Where(x => x.IsActive)
                    .Sum(x => x.MonthlyRent);

            decimal previousDue = await _context.tblTenantBill
     .Where(x => x.TenantId == dto.TenantId)
     .OrderByDescending(x => x.BillId)//orOrderByDescending(x => x.BillDate) //
     .Select(x => x.RemainingDue)
     .FirstOrDefaultAsync();

            decimal total =
                monthlyRent +
                dto.ElectricityCharge +
                dto.WaterCharge +
                dto.GarbageCharge +
                dto.InternetCharge +
                previousDue;

            var bill = new TenantBillModel
            {
                TenantId = dto.TenantId,
                Year = dto.Year,
                Month = dto.Month,

                RentAmount = monthlyRent,
                ElectricityCharge = dto.ElectricityCharge,
                WaterCharge = dto.WaterCharge,
                GarbageCharge = dto.GarbageCharge,
                InternetCharge = dto.InternetCharge,

                PreviousDue = previousDue,

                TotalAmount = total,
                RemainingDue = total,

                PaidAmount = 0,
                IsPaid = false,

                BillDate = DateTime.Now
            };

            await _context.tblTenantBill.AddAsync(bill);

            await _context.SaveChangesAsync();

            return Ok(bill);
        }

        [HttpPost("PayBill")]
        public async Task<IActionResult> PayBill(
    PayBillDto dto)
        {
            var bill =
                await _context.tblTenantBill
                    .FirstOrDefaultAsync(x =>
                        x.BillId == dto.BillId);

            if (bill == null)
                return NotFound();

            bill.PaidAmount += dto.PaidAmount;

            bill.RemainingDue =
                bill.TotalAmount - bill.PaidAmount;

            if (bill.RemainingDue <= 0)
            {
                bill.IsPaid = true;
                bill.RemainingDue = 0;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                bill.BillId,
                bill.TotalAmount,
                bill.PaidAmount,
                bill.RemainingDue,
                bill.IsPaid
            });
        }
        [HttpGet("BillDetail/{billId}")]
        public async Task<IActionResult> BillDetail(int billId)
        {
            var bill =
                await _context.tblTenantBill
                    .Include(x => x.Tenant)
                    .FirstOrDefaultAsync(x =>
                        x.BillId == billId);

            if (bill == null)
                return NotFound();

            return Ok(bill);
        }

        [HttpGet("TenantBills/{tenantId}")]
        public async Task<IActionResult> TenantBills(int tenantId)
        {
            var bills = await _context.tblTenantBill
                .Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.BillDate)
                .ToListAsync();

            return Ok(bills);
        }
    }
}
