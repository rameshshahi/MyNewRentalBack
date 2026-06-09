using NewRentalApi.DTOs;

namespace NewRentalApi.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);

        Task<object> LoginAsync(LoginDto dto);
    }
}
