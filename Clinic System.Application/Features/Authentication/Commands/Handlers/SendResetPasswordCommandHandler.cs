namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class SendResetPasswordCommandHandler : ResponseHandler, IRequestHandler<SendResetPasswordCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly ILogger<SendResetPasswordCommandHandler> _logger;

        public SendResetPasswordCommandHandler(
            IIdentityService identityService,
            IEmailService emailService,
            ILogger<SendResetPasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Password reset requested for email: {Email}", request.Email);

            try
            {
                // 1. توليد التوكن
                var token = await _identityService.GeneratePasswordResetTokenAsync(request.Email);

                // 2. تشفير التوكن
                var encodedToken = _identityService.EncodeToken(token);

                // 3. تجهيز اللينك
                var resetLink = $"{request.BaseUrl}/api/authentication/reset-password?email={request.Email}&code={encodedToken}";

                // 4. تجهيز التيمبليت
                var emailBody = EmailTemplates.GetResetPasswordTemplate(request.Email, resetLink);

                // 5. الإرسال
                await _emailService.SendEmailAsync(request.Email, "Elite Clinic - Password Reset Request", emailBody);

                _logger.LogInformation("Password reset email sent successfully to: {Email}", request.Email);

                return Success("Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling password reset for email: {Email}", request.Email);
                // رجع رسالة خطأ شيك بدل ما السيرفر يضرب
                return BadRequest<string>("Failed to send reset email. Please try again later or check the email address.");
            }
        }
    }
}
