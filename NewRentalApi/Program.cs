using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewRentalApi.Data;
using NewRentalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your React app URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddScoped<RentalDbContext>(provider =>
{
    var tenantProvider =
        provider.GetRequiredService<ITenantProvider>();

    var databaseName =
        tenantProvider.DatabaseName;

    var connectionString =
        $"Server=ramesh-PC\\SqlExpress;uid=sa;pwd=sql;Database={databaseName};TrustServerCertificate=True;";

    var options =
        new DbContextOptionsBuilder<RentalDbContext>()
            .UseSqlServer(connectionString)
            .Options;

    return new RentalDbContext(options);
});

builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MasterConnection")));
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]))
        };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
