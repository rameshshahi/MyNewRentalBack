using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRentalApi.Data;
using NewRentalApi.DTOs;
using NewRentalApi.Models;
using NewRentalApi.Services;
using Newtonsoft.Json;
namespace NewRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly RentalDbContext _context;
        private readonly HttpClient _httpClient;
        private const string KhaltiSecretKey = "804be30cbb4e412cb39e10f5f9a98748"; // Replace with your test secret key
        public BillController(RentalDbContext context)
        {
            _httpClient = new HttpClient();
            _context = context;
        }



            
        [HttpPost("GenerateBill")]
        public async Task<IActionResult> GenerateBill(GenerateBillDto dto)
        {
            if (dto.Year != DateTime.Now.Year || dto.Month != DateTime.Now.Month)
            {
                return BadRequest("Bills can only be generated for the current year and current month.");
            }


            var tenant = await _context.tblTenant
                .Include(x => x.TenantRooms)
                .FirstOrDefaultAsync(x =>
                    x.TenantId == dto.TenantId);

            if (tenant == null)
                return NotFound("Tenant not found");

            var existingBill = await _context.tblTenantBill
                   .AnyAsync(x => x.TenantId == dto.TenantId &&
                   x.Month == dto.Month &&
                   x.Year == dto.Year);

            if (existingBill)
                return BadRequest("Bill already exists for this month.");

            var activeRooms = tenant.TenantRooms.Where(x => x.IsActive).ToList();

            if (!activeRooms.Any())
                return BadRequest("Tenant has no active rooms. Cannot generate bill.");

            decimal monthlyRent = activeRooms.Sum(x => x.MonthlyRent);

            decimal previousDue = await _context.tblTenantBill
                .Where(x => x.TenantId == dto.TenantId)
                .OrderByDescending(x => x.BillId)
                .Select(x => (decimal?)x.RemainingDue)
                .FirstOrDefaultAsync() ?? 0;

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


        [HttpPost("esewa-initiate")]
        public IActionResult InitiatePayment([FromBody] PaymentRequest request)
        {
            var transactionId = Guid.NewGuid().ToString();

            var signature = GenerateSignature(
                request.Amount.ToString(),
                transactionId,
                "EPAYTEST");

            var formData = new
            {
                amount = request.Amount,
                tax_amount = 0,
                total_amount = request.Amount,
                transaction_uuid = transactionId,
                product_code = "EPAYTEST",
                product_service_charge = 0,
                product_delivery_charge = 0,
                success_url =
                    "https://localhost:3000/payment-success",
                failure_url =
                    "https://localhost:3000/payment-failure",
                signed_field_names =
                    "total_amount,transaction_uuid,product_code",
                signature = signature
            };

            return Ok(new
            {
                paymentUrl =
                    "https://rc-epay.esewa.com.np/api/epay/main/v2/form",
                formData
            });
        }
        [HttpGet("verify")]
        public async Task<IActionResult> Verify(string transactionUuid, decimal amount)
        {
            var url =
                $"https://rc.esewa.com.np/api/epay/transaction/status/" +
                $"?product_code=EPAYTEST" +
                $"&total_amount={amount}" +
                $"&transaction_uuid={transactionUuid}";

            var client = new HttpClient();

            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        public static string GenerateSignature(string totalAmount, string transactionUuid, string productCode)
        {
            string secretKey = "8gBm/:&EnhH.1/q"; // Test Secret

            string message =
                $"total_amount={totalAmount}," +
                $"transaction_uuid={transactionUuid}," +
                $"product_code={productCode}";

            var keyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);

            var hash = hmac.ComputeHash(messageBytes);

            return Convert.ToBase64String(hash);
        }

        [HttpPost("khalti-initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] KhaltiPaymentRequest request)
        {
            var payload = new
            {
                return_url = "http://localhost:3000/payment-success", // Your frontend URL
                website_url = "http://localhost:3000",
                amount = request.Amount,
                purchase_order_id = request.ProductIdentity,
                purchase_order_name = request.ProductName,
            };

            var json = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://a.khalti.com/api/v2/epayment/initiate/");
            httpRequest.Content = httpContent;
            httpRequest.Headers.Add("Authorization", $"Key {KhaltiSecretKey}");

            var response = await _httpClient.SendAsync(httpRequest);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Optionally log the error for debugging
                return BadRequest(new { error = result });
            }

            dynamic res = JsonConvert.DeserializeObject(result);

            // Check if pidx exists in the response
            if (res?.pidx == null)
            {
                return BadRequest(new { error = "Khalti did not return a pidx." });
            }

            // Return pidx as token and also return the payment_url for frontend redirection
            return Ok(new { token = res.pidx.ToString(), payment_url = res.payment_url.ToString() });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] string pidx)
        {
            var json = JsonConvert.SerializeObject(new { pidx });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://a.khalti.com/api/v2/epayment/lookup/");
            httpRequest.Content = httpContent;
            httpRequest.Headers.Add("Authorization", $"Key {KhaltiSecretKey}");

            var response = await _httpClient.SendAsync(httpRequest);
            var result = await response.Content.ReadAsStringAsync();

            return Ok(JsonConvert.DeserializeObject(result));
        }

        [HttpPost("VerifyPayment")]
        public async Task<IActionResult> VerifyPayment(VerifyPaymentDto dto)
        {
            var bill = await _context.tblTenantBill
                .FirstOrDefaultAsync(x => x.BillId == dto.BillId);

            if (bill == null)
                return NotFound("Bill not found.");

            decimal paidAmount = 0;
            string transactionId = "";
            string paymentStatus = "";

            //---------------------------------------
            // KHALTI
            //---------------------------------------
            if (dto.PaymentGateway == "Khalti")
            {
                var json = JsonConvert.SerializeObject(new
                {
                    pidx = dto.Pidx
                });

                var request =
                    new HttpRequestMessage(
                        HttpMethod.Post,
                        "https://a.khalti.com/api/v2/epayment/lookup/");

                request.Content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                request.Headers.Add(
                    "Authorization",
                    $"Key {KhaltiSecretKey}");

                var response =
                    await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Khalti verification failed.");

                var result =
                    await response.Content.ReadAsStringAsync();

                dynamic payment =
                    JsonConvert.DeserializeObject(result);

                if (payment.status != "Completed")
                    return BadRequest("Payment not completed.");

                paidAmount =
                    Convert.ToDecimal(payment.total_amount) / 100;

                transactionId =
                    payment.transaction_id;

                paymentStatus =
                    payment.status;
            }

            //---------------------------------------
            // ESEWA
            //---------------------------------------
            else if (dto.PaymentGateway == "Esewa")
            {
                var response =
                    await _httpClient.GetAsync(
                        $"https://rc.esewa.com.np/api/epay/transaction/status/?product_code=EPAYTEST&total_amount={bill.TotalAmount}&transaction_uuid={dto.TransactionUuid}");

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Esewa verification failed.");

                var json =
                    await response.Content.ReadAsStringAsync();

                dynamic payment =
                    JsonConvert.DeserializeObject(json);

                if (payment.status != "COMPLETE")
                    return BadRequest("Payment not completed.");

                paidAmount =
                    Convert.ToDecimal(payment.total_amount);

                transactionId =
                    payment.transaction_code;

                paymentStatus =
                    payment.status;
            }
            else
            {
                return BadRequest("Invalid Payment Gateway.");
            }

            //---------------------------------------
            // Prevent Duplicate Payment
            //---------------------------------------

            bool exists =
                await _context.tblTenantPayment
                    .AnyAsync(x =>
                        x.TransactionId == transactionId);

            if (exists)
                return BadRequest("Payment already verified.");

            //---------------------------------------
            // Insert Payment History
            //---------------------------------------

            var paymentHistory =
                new TenantPaymentModel
                {
                    BillId = bill.BillId,
                    TenantId = bill.TenantId,
                    Amount = paidAmount,
                    PaymentGateway = dto.PaymentGateway,
                    TransactionId = transactionId,
                    Pidx = dto.Pidx,
                    Status = paymentStatus,
                    PaymentDate = DateTime.Now,
                    Remarks = "Online Payment"
                };

            await _context.tblTenantPayment
                .AddAsync(paymentHistory);

            //---------------------------------------
            // Update Bill
            //---------------------------------------

            bill.PaidAmount += paidAmount;

            bill.RemainingDue =
                bill.TotalAmount -
                bill.PaidAmount;

            if (bill.RemainingDue <= 0)
            {
                bill.IsPaid = true;
                bill.RemainingDue = 0;
            }

            await _context.SaveChangesAsync();

            //---------------------------------------

            return Ok(new
            {
                Success = true,
                Message = "Payment Verified Successfully",
                BillId = bill.BillId,
                bill.TotalAmount,
                bill.PaidAmount,
                bill.RemainingDue,
                bill.IsPaid
            });
        }
    }
}
