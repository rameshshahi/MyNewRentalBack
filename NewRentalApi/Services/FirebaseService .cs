using FirebaseAdmin.Messaging;

namespace NewRentalApi.Services
{
    public class FirebaseService : IFirebaseService
    {
        public async Task SendNotification(
            string deviceToken,
            string title,
            string body)
        {
            var message = new Message
            {
                Token = deviceToken,

                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance
                .SendAsync(message);
        }
    }
}
