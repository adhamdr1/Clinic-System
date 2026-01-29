namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientByIdQueryHandler : AppRequestHandler<GetPatientByIdQuery, GetPatientDTO>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientByIdQueryHandler> logger;

        public PatientByIdQueryHandler(
            ICurrentUserService currentUserService, // <---
            IPatientService patientService,
            IMapper mapper,
            ILogger<PatientByIdQueryHandler> logger) : base(currentUserService)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<GetPatientDTO>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPatientByIdQuery for ID: {Id}", request.Id);

            var authResult = await ValidatePatientAccess(request.Id);

            if (authResult != null)
                return authResult;

            var patient = await patientService.GetPatientByIdAsync(request.Id, cancellationToken);

            if (patient == null)
            {
                logger.LogWarning("Patient with ID: {Id} not found.", request.Id);
                return NotFound<GetPatientDTO>();
            }

            var patientsMapper = mapper.Map<GetPatientDTO>(patient);

            logger.LogInformation("Successfully retrieved patient with ID: {Id}", request.Id);
            return Success(patientsMapper);
        }
    }
}
