namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class CompleteGoogleRegistrationCommandHandler : ResponseHandler, IRequestHandler<CompleteGoogleRegistrationCommand, Response<AuthDTO>>
    {
        private readonly IIdentityService _identityService;
        private readonly IPatientService _patientService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CompleteGoogleRegistrationCommandHandler> _logger;

        public CompleteGoogleRegistrationCommandHandler(
            IIdentityService identityService,
            IPatientService patientService,
            IAuthenticationService authenticationService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CompleteGoogleRegistrationCommandHandler> logger)
        {
            _identityService = identityService;
            _patientService = patientService;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<AuthDTO>> Handle(CompleteGoogleRegistrationCommand request, CancellationToken cancellationToken)
        {
            string userId = string.Empty;
            Patient patient = null;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _logger.LogInformation("Starting Google registration completion for {Email}", request.Email);

                    userId = await _identityService.CreateUserForGoogleAsync(
                        userName: request.Email,
                        email: request.Email,
                        role: "Patient",
                        cancellationToken);

                    patient = _mapper.Map<Patient>(request);
                    patient.ApplicationUserId = userId;

                    await _patientService.CreatePatientAsync(patient, cancellationToken);
                    var result = await _unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        _logger.LogWarning("Failed to save patient {Email} to the database.", request.Email);
                        return BadRequest<AuthDTO>("Failed to complete registration.");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while completing Google registration for {Email}", request.Email);
                    return BadRequest<AuthDTO>($"Registration completion failed: {ex.Message}");
                }
            }

            try
            {
                var customClaims = new List<Claim>
                {
                    new Claim("PatientId", patient.Id.ToString())
                };

                var roles = new List<string> { "Patient" };

                var (accessToken, refreshToken, expiresAt, _, _, _) =
                    await _authenticationService.GenerateJwtTokenAsync(
                        userId,
                        request.Email,
                        request.Email,
                        roles,
                        customClaims);

                var response = new AuthDTO
                {
                    IsSuccess = true,
                    Message = "Registration completed and logged in successfully.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    RequiresCompletion = false
                };

                _logger.LogInformation("Patient {Email} completed Google registration and logged in.", request.Email);
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate token after completing Google registration for {Email}", request.Email);
                return Failure<AuthDTO>("Registration completed but failed to automatically log in. Please try logging in manually.");
            }
        }
    }
}
