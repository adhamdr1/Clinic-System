namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class ResendConfirmationEmailCommandHandler : ResponseHandler, IRequestHandler<ResendConfirmationEmailCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendConfirmationEmailCommandHandler> _logger;

        public ResendConfirmationEmailCommandHandler(
            IIdentityService identityService,
            IEmailService emailService,
            ILogger<ResendConfirmationEmailCommandHandler> logger)
        {
            _identityService = identityService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var (userId, userName, role, token, error) = await _identityService.GenerateTokenForResendEmailConfirmationAsync(request.Email);

            if (!string.IsNullOrEmpty(error))
                return BadRequest<string>(error);

            try
            {
                var encodedToken = _identityService.EncodeToken(token);

                var confirmationLink = $"{request.BaseUrl}/api/authentication/confirm-email?userId={userId}&code={encodedToken}";


                var emailBody = EmailTemplates.GetEmailConfirmationTemplate(
                    userName, 
                    userName, 
                    request.Email,
                    confirmationLink,
                    role
                );

                await _emailService.SendEmailAsync(request.Email, "Resend Confirmation - Elite Clinic", emailBody);

                return Success("Confirmation email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resend confirmation email to {Email}", request.Email);
                return BadRequest<string>("Failed to send email. Please try again.");
            }
        }
    }
}
