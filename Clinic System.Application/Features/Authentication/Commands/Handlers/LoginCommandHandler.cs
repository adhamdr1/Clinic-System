namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class LoginCommandHandler : ResponseHandler,IRequestHandler<LoginCommand, Response<LoginResponseDTO>>
    {
        private readonly IIdentityService identityService;
        private readonly IAuthenticationService authenticationService;
        private readonly IUnitOfWork unitOfWork; // 1. زودنا الـ UoW
        private readonly ILogger<LoginCommandHandler> logger;

        public LoginCommandHandler(
            IIdentityService identityService,
            IAuthenticationService authenticationService,
            IUnitOfWork unitOfWork, // الحقن هنا
            ILogger<LoginCommandHandler> logger)
        {
            this.identityService = identityService;
            this.authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            int id = 0; // just to test code formatting in the playground
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

                var customClaims = new List<Claim>();

                if (Roles.Contains("Doctor"))
                {
                    // بنروح نجيب الـ Doctor ID من الداتابيز
                    var doctor = await unitOfWork.DoctorsRepository.GetDoctorByUserIdAsync(Id);
                    if (doctor != null)
                    {
                        customClaims.Add(new Claim("DoctorId", doctor.Id.ToString()));
                    }
                    id = doctor.Id;
                }
                else if (Roles.Contains("Patient"))
                {
                    // بنروح نجيب الـ Patient ID من الداتابيز
                    var patient = await unitOfWork.PatientsRepository.GetPatientByUserIdAsync(Id);
                    if (patient != null)
                    {
                        customClaims.Add(new Claim("PatientId", patient.Id.ToString()));
                    }
                    id = patient.Id;
                }

                var (accesstoken, refreshtoken, expiresAt, userName, email,roles) =
                await authenticationService.GenerateJwtTokenAsync(Id, UserName, Email, Roles, customClaims);

                logger.LogInformation("User {EmailOrUserName} authenticated successfully.", request.EmailOrUserName);


                var response = new LoginResponseDTO
                {
                    Id = id,
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
