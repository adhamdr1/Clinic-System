namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class ResendConfirmationEmailCommandHandler : ResponseHandler, IRequestHandler<ResendConfirmationEmailCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<ResendConfirmationEmailCommandHandler> _logger;

        public ResendConfirmationEmailCommandHandler(
            IIdentityService identityService,
            IMessagePublisher messagePublisher,
            ILogger<ResendConfirmationEmailCommandHandler> logger)
        {
            _identityService = identityService;
            _messagePublisher = messagePublisher;
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


                await _messagePublisher.PublishAsync(new UserRegisteredEvent
                {
                    UserId = userId,
                    FullName = userName, // بنستخدم الـ UserName كاسم مبدئي هنا
                    UserName = userName,
                    Email = request.Email,
                    ConfirmationLink = confirmationLink,
                    UserRole = role,
                    Specialty = null
                }, cancellationToken);

                _logger.LogInformation("Resend confirmation event published to RabbitMQ for {Email}", request.Email);

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
