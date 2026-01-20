namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class RefreshTokenCommandHandler : ResponseHandler, IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IAuthenticationService authenticationService, ILogger<RefreshTokenCommandHandler> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start handling RefreshTokenCommand for AccessToken: {AccessToken}", request.AccessToken);

            try
            {
                var (accessToken, refreshToken, expiresAt) = await _authenticationService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning("Failed to refresh token. Invalid or expired refresh token provided.");
                    return Unauthorized<JwtAuthResult>("Invalid or Expired Refresh Token");
                }

                var response = new JwtAuthResult
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt.ToString("yyyy-MM-dd HH:mm:ss") 
                };

                _logger.LogInformation("Token refreshed successfully for user.");

                return Success(response, "Token Refreshed Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while refreshing the token.");

                return Unauthorized<JwtAuthResult>("An error occurred while processing your request.");
            }
        }
    }
}
