namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class LoginCommandHandler : ResponseHandler,IRequestHandler<LoginCommand, Response<LoginResponseDTO>>
    {
        private readonly IIdentityService identityService;
        private readonly IAuthenticationService authenticationService;
        private readonly ILogger<LoginCommandHandler> logger;
        public LoginCommandHandler(IIdentityService identityService, IAuthenticationService authenticationService, ILogger<LoginCommandHandler> logger)
        {
            this.identityService = identityService;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        public async Task<Response<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (IsAuthenticated, IsEmailConfirmed , Id, UserName, Email, Roles) = await identityService.LoginAsync(request.EmailOrUserName, request.Password);

                if (!IsAuthenticated)
                {
                    logger.LogWarning("Authentication failed for user: {EmailOrUserName}", request.EmailOrUserName);
                    return Unauthorized<LoginResponseDTO>("Invalid credentials provided.");
                }

                if (!IsEmailConfirmed) 
                {
                    logger.LogWarning("Email not confirmed for user: {EmailOrUserName}", request.EmailOrUserName);
                    return Failure<LoginResponseDTO>("Email address is not confirmed.");
                }

                var (accesstoken, refreshtoken, expiresAt, userName, email,roles) =
                await authenticationService.GenerateJwtTokenAsync(Id, UserName, Email, Roles);

                logger.LogInformation("User {EmailOrUserName} authenticated successfully.", request.EmailOrUserName);


                var response = new LoginResponseDTO
                {
                    UserName = userName ?? string.Empty,
                    Email = email ?? string.Empty,
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken,
                    ExpiresAt = expiresAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    Roles = roles ?? new List<string>()
                };

                return Success(response, "Login Successful");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the login command.");
                return Failure<LoginResponseDTO>("An unexpected error occurred during login.");
            }
        }
    }
}
