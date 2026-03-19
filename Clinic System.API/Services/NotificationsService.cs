
namespace Clinic_System.API.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToAllAsync(NotificationDTO notification)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public async Task SendToUserAsync(string userId, NotificationDTO notification)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        public async Task SendToGroupAsync(string groupName, NotificationDTO notification)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", notification);
        }
    }
}
