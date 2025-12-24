namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientListByNameQueryHandler : ResponseHandler, IRequestHandler<GetPatientListByNameQuery, Response<List<GetPatientListDTO>>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientListByNameQueryHandler> logger;

        public PatientListByNameQueryHandler(IPatientService patientService,
            IMapper mapper,
            ILogger<PatientListByNameQueryHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<List<GetPatientListDTO>>> Handle(GetPatientListByNameQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPatientListByNameQuery for Name: {Name}", request.FullName);

            var patients = await patientService.GetPatientListByNameAsync(request.FullName, cancellationToken);

            if (patients?.Any() != true)
            {
                logger.LogWarning("No patients found for Name: {Name}", request.FullName);
                return NotFound<List<GetPatientListDTO>>($"No patients found with Name: {request.FullName}");
            }

            var patientsMapper = mapper.Map<List<GetPatientListDTO>>(patients);

            logger.LogInformation("Found {Count} patients for Name: {Name}", patientsMapper.Count, request.FullName);
            return Success(patientsMapper);
        }
    }
}
