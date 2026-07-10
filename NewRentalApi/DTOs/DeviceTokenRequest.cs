namespace NewRentalApi.DTOs
{
    
        public class DeviceTokenRequest
        {
            public int UserId { get; set; }
            public string UserType { get; set; } = "";   // Owner or Tenant
            public string DeviceToken { get; set; } = "";
        }
    
}
