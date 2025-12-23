namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientListQueryHandler : ResponseHandler, IRequestHandler<GetPatientListQuery, Response<List<GetPatientListDTO>>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientListQueryHandler> logger;

        public PatientListQueryHandler(IPatientService patientService,
            IMapper mapper,
            ILogger<PatientListQueryHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<List<GetPatientListDTO>>> Handle(GetPatientListQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPatientListQuery");

            var patients = await patientService.GetPatientsListAsync(cancellationToken);

            if (patients?.Any() != true)
            {
                logger.LogWarning("No patients found");
                return NotFound<List<GetPatientListDTO>>();
            }

            var patientsMapper = mapper.Map<List<GetPatientListDTO>>(patients);

            logger.LogInformation("Successfully retrieved and mapped patient list");

            return Success(patientsMapper);
        }
    }
}
