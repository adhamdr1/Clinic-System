namespace Clinic_System.Infrastructure.Services.Email
{
    public class IdentityNotificationService : IIdentityNotificationService
    {
        private readonly IEmailService _emailService;

        public IdentityNotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailConfirmationAsync(string userId, string name, string userName, string email, string link, string role, string? specialty)
        {
            var subject = "Welcome to Elite Clinic - Confirm Your Account";

            // بنستخدم الـ Template بتاعك اللي بياخد الـ Parameters دايركت
            var body = EmailTemplates.GetEmailConfirmationTemplate(name, userName, email, link, role, specialty);

            await _emailService.SendEmailAsync(email, subject, body);
        }

        public async Task SendResetPasswordAsync(string email, string link)
        {
            var subject = "Elite Clinic - Reset Your Password";

            var body = EmailTemplates.GetResetPasswordTemplate(email, link);

            await _emailService.SendEmailAsync(email, subject, body);
        }
    }
}
