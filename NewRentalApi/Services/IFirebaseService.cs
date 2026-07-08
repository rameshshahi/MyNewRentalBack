namespace NewRentalApi.Services
{
    
        public interface IFirebaseService
        {
            Task SendNotification(
                string deviceToken,
                string title,
                string body);
        }
    
}
