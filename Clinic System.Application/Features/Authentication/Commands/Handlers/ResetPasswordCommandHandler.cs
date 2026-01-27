namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class ResetPasswordCommandHandler : ResponseHandler, IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IIdentityService identityService, ILogger<ResetPasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // الـ FluentValidation هيشتغل أوتوماتيك قبل ما يدخل هنا
            // فمش محتاجين نشيك على الـ Match Password هنا تاني

            try
            {
                // 1. فك التشفير
                var decodedCode = _identityService.DecodeToken(request.Code);

                // 2. محاولة التغيير واستقبال النتيجة التفصيلية
                var (succeeded, error) = await _identityService.ResetPasswordAsync(request.Email, decodedCode, request.NewPassword);

                if (succeeded)
                {
                    _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);
                    return Success("Password has been changed successfully.");
                }

                // 3. عرض الخطأ المحدد اللي جه من الـ Identity
                // هنا ممكن يكون: "Invalid Token" أو "Passwords must have at least one digit"
                return BadRequest<string>(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", request.Email);
                return Failure<string>("An unexpected error occurred.");
            }
        }
    }
}
