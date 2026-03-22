namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class GoogleLoginCommandHandler : ResponseHandler, IRequestHandler<GoogleLoginCommand, Response<AuthDTO>>
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IIdentityService _identityService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GoogleLoginCommandHandler> _logger;

        public GoogleLoginCommandHandler(
            IGoogleAuthService googleAuthService,
            IIdentityService identityService,
            IAuthenticationService authenticationService,
            IUnitOfWork unitOfWork,
            ILogger<GoogleLoginCommandHandler> logger)
        {
            _googleAuthService = googleAuthService;
            _identityService = identityService;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<AuthDTO>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Google Login processing...");

            // 1. Verify Google Token
            var googleUser = await _googleAuthService.VerifyGoogleTokenAsync(request.IdToken);

            if (googleUser == null)
            {
                _logger.LogWarning("Invalid Google Token provided.");
                return BadRequest<AuthDTO>("Invalid Google Token.");
            }

            // 2. Check if user exists in our database
            var (exists, userId, userName, roles) = await _identityService.GetUserDetailsByEmailForGoogleAsync(googleUser.Email);

            if (exists)
            {
                // Security Check: Only allow Patients to login via Google
                if (!roles.Contains("Patient"))
                {
                    _logger.LogWarning("Unauthorized Google login attempt by non-patient: {Email}", googleUser.Email);
                    return Unauthorized<AuthDTO>("Only patients are allowed to login via Google.");
                }

                var customClaims = new List<Claim>();
                var patient = await _unitOfWork.PatientsRepository.GetPatientByUserIdAsync(userId);

                if (patient != null)
                    customClaims.Add(new Claim("PatientId", patient.Id.ToString()));

                // Generate Token
                var (accessToken, refreshToken, expiresAt, _, _, _) =
                    await _authenticationService.GenerateJwtTokenAsync(userId, userName, googleUser.Email, roles, customClaims);

                var response = new AuthDTO
                {
                    IsSuccess = true,
                    Message = "Login successful.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    RequiresCompletion = false
                };

                _logger.LogInformation("Patient {Email} logged in successfully via Google.", googleUser.Email);
                return Success(response);
            }
            else
            {
                // New Patient Case
                var response = new AuthDTO
                {
                    IsSuccess = true,
                    Message = "Please complete registration to continue.",
                    RequiresCompletion = true,
                    GoogleEmail = googleUser.Email,
                    GoogleName = googleUser.Name
                };

                _logger.LogInformation("New patient from Google. Requesting profile completion for {Email}.", googleUser.Email);
                return Success(response, "User needs to complete registration.");
            }
        }
    }
}
