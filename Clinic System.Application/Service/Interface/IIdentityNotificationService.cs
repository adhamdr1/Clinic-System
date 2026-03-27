namespace Clinic_System.Application.Service.Interface
{
    public interface IIdentityNotificationService
    {
        Task SendEmailConfirmationAsync(string userId, string name, string userName, string email, string link, string role, string? specialty);
        Task SendResetPasswordAsync(string email, string link);
    }
}
