namespace Clinic_System.Application.Service.Interface
{
    public interface INotificationsService
    {
        Task SendToAllAsync(NotificationDTO notification);
        Task SendToUserAsync(string userId, NotificationDTO notification);
        Task SendToGroupAsync(string groupName, NotificationDTO notification);
    }
}
