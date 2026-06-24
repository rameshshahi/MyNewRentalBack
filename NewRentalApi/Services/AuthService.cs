using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewRentalApi.Data;
using NewRentalApi.DTOs;
using NewRentalApi.Models;
using NewRentalApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace NewRentalApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly MasterDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            MasterDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _context.tblOwner
                .AnyAsync(x => x.PhoneNo == dto.PhoneNo))
            {
                throw new Exception("Phone already exists.");
            }

            string dbName =
                $"Rental_Owner_{Guid.NewGuid():N}";

            var owner = new OwnerModel
            {
                OwnerName = dto.OwnerName,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                DatabaseName = dbName,
                CreatedDate = DateTime.UtcNow
            };

            _context.tblOwner.Add(owner);

            await _context.SaveChangesAsync();

        

            // Create database
            await CreateDatabaseAsync(dbName);

            // Run migrations
            await ApplyRentalMigrationAsync(dbName);

            

            await _context.SaveChangesAsync();

            return "Registration successful";
        }

        public async Task<object> LoginAsync(LoginDto dto)
        {
            var owner = await _context.tblOwner
                .FirstOrDefaultAsync(x =>
                    x.PhoneNo == dto.PhoneNo);

            if (owner == null)
                throw new Exception("Invalid Phone.");

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    owner.PasswordHash);

            if (!validPassword)
                throw new Exception("Invalid password.");

            var token = GenerateJwtToken(owner);

            return new
            {
                Token = token,
                OwnerId = owner.OwnerId,
                OwnerName = owner.OwnerName,
                DatabaseName = owner.DatabaseName
            };
        }
        private async Task CreateDatabaseAsync(string databaseName)
        {
            string masterConnection =
                _configuration.GetConnectionString("MasterConnection");

            using SqlConnection connection =
                new SqlConnection(masterConnection);

            await connection.OpenAsync();

            string sql =
                $"IF DB_ID('{databaseName}') IS NULL CREATE DATABASE [{databaseName}]";

            using SqlCommand command =
                new SqlCommand(sql, connection);

            await command.ExecuteNonQueryAsync();
        }

        private async Task ApplyRentalMigrationAsync(string databaseName)
        {
            string connectionString =
                $"Server=ramesh-PC\\SqlExpress;Database={databaseName};User Id=sa;Password=sql;TrustServerCertificate=True;";

            var options =
                new DbContextOptionsBuilder<RentalDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            using var rentalContext =
                new RentalDbContext(options);

            //await rentalContext.Database.MigrateAsync();
            await rentalContext.Database.EnsureCreatedAsync();
        }
        private string GenerateJwtToken(OwnerModel owner)
        {
            var claims = new[]
            {
            new Claim("OwnerId", owner.OwnerId.ToString()),
            new Claim("DatabaseName", owner.DatabaseName),
            new Claim(ClaimTypes.Name, owner.OwnerName),
            new Claim(ClaimTypes.Email, owner.Email),
            new Claim("PhoneNo", owner.PhoneNo.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }

}
   