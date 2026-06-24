using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewRentalApi.Data;
using NewRentalApi.DTOs;
using NewRentalApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace NewRentalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantAuthController : ControllerBase
    {
        private readonly RentalDbContext _context;
        private readonly MasterDbContext _masterContext;
        private readonly IConfiguration _configuration;

        public TenantAuthController(
            RentalDbContext context,
            MasterDbContext masterContext,
            IConfiguration configuration)
        {
            _context = context;
            _masterContext = masterContext;
            _configuration = configuration;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpDto dto)
        {
            var tenantLogin =
                await _masterContext.tblTenantLogin
                    .FirstOrDefaultAsync(x =>
                        x.PhoneNo == dto.PhoneNumber &&
                        x.IsActive);

            if (tenantLogin == null)
                return NotFound("Tenant not found");

            //var otp = new Random()
            //    .Next(100000, 999999)
            //    .ToString();

            var otp = 123456.ToString();
            var tenantOtp = new TenantOtp
            {
                PhoneNumber = dto.PhoneNumber,
                OtpCode = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            _masterContext.TenantOtps.Add(tenantOtp);

            await _masterContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "OTP Sent",
                OTP = otp
            });
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var otpRecord =
                await _masterContext.TenantOtps
                    .Where(x =>
                        x.PhoneNumber == dto.PhoneNumber &&
                        x.OtpCode == dto.Otp &&
                        !x.IsUsed)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

            if (otpRecord == null)
                return BadRequest("Invalid OTP");

            if (otpRecord.ExpiryTime < DateTime.Now)
                return BadRequest("OTP Expired");

            otpRecord.IsUsed = true;

            var tenantLogin =
                await _masterContext.tblTenantLogin
                    .FirstOrDefaultAsync(x =>
                        x.PhoneNo == dto.PhoneNumber &&
                        x.IsActive);

            if (tenantLogin == null)
                return NotFound("Tenant not found");

            await _masterContext.SaveChangesAsync();

            var token =
                GenerateTenantToken(tenantLogin);

            return Ok(new
            {
                Message = "Login Successful",
                Token = token,
                TenantId = tenantLogin.TenantId,
                TenantName = tenantLogin.FullName
            });
        }

        private string GenerateTenantToken(TenantLoginModel tenant)
        {
            var claims = new[]
            {
        new Claim(
            ClaimTypes.NameIdentifier,
            tenant.TenantId.ToString()),

        new Claim(
            ClaimTypes.Name,
            tenant.FullName),

        new Claim(
            ClaimTypes.Role,
            "Tenant"),

        new Claim(
            "PhoneNumber",
            tenant.PhoneNo),

        new Claim(
            "DatabaseName",
            tenant.DatabaseName),

        new Claim(
            "OwnerId",
            tenant.OwnerId.ToString())
    };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],
                    audience:
                        _configuration["Jwt:Audience"],
                    claims:
                        claims,
                    expires:
                        DateTime.Now.AddDays(7),
                    signingCredentials:
                        creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}