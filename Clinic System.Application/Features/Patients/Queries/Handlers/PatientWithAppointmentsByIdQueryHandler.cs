namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientWithAppointmentsByIdQueryHandler : AppRequestHandler<GetPatientWithAppointmentsByIdQuery, GetPatientWhitAppointmentDTO>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ILogger<PatientWithAppointmentsByIdQueryHandler> logger;

        public PatientWithAppointmentsByIdQueryHandler(
            ICurrentUserService currentUserService, 
            IPatientService patientService,
            IMapper mapper,
            IIdentityService identityService,
            ILogger<PatientWithAppointmentsByIdQueryHandler> logger) : base(currentUserService)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async override Task<Response<GetPatientWhitAppointmentDTO>> Handle(GetPatientWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {

            var patient = await patientService.GetPatientWithAppointmentsByIdAsync(request.Id, cancellationToken);

            if (patient == null)
            {
                logger.LogInformation("GetPatientWithAppointmentsByIdQueryHandler: Patient with ID {Id} not found", request.Id);
                return NotFound<GetPatientWhitAppointmentDTO>($"Patient with ID {request.Id} not found");
            }

            var authResult = await ValidateOwner(patient.ApplicationUserId);
            if (authResult != null) return authResult;

            var patientsMapper = mapper.Map<GetPatientWhitAppointmentDTO>(patient);

            if (!string.IsNullOrEmpty(patient.ApplicationUserId))
            {
                var (email, userName) = await identityService.GetUserEmailAndUserNameAsync(patient.ApplicationUserId, cancellationToken);

                patientsMapper.Email = email;
                patientsMapper.UserName = userName;

                if (string.IsNullOrEmpty(patientsMapper.Email) || string.IsNullOrEmpty(patientsMapper.UserName))
                {
                    logger.LogWarning("Missing Identity data for Doctor AppUserId: {AppUserId}", patient.ApplicationUserId);
                }
            }

            logger.LogInformation("GetPatientWithAppointmentsByIdQueryHandler: Successfully retrieved patient with ID {Id}", request.Id);

            return Success(patientsMapper);
        }
    }
}