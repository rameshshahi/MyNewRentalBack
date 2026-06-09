namespace NewRentalApi.Services
{
    public class TenantProvider:ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string DatabaseName
        {
            get
            {
                return _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirst("DatabaseName")
                    ?.Value;
            }
        }
    }
}
