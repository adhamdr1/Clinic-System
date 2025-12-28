namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientByPhoneQueryHandler : ResponseHandler, IRequestHandler<GetPatientByPhoneQuery, Response<GetPatientDTO>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientByPhoneQueryHandler> logger;

        public PatientByPhoneQueryHandler(
            IPatientService patientService,
            IMapper mapper,
            ILogger<PatientByPhoneQueryHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<GetPatientDTO>> Handle(GetPatientByPhoneQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPatientByPhoneQuery for Phone: {Phone}", request.Phone);

            var patient = await patientService.GetPatientByPhoneAsync(request.Phone, cancellationToken);

            if (patient == null)
            {
                logger.LogWarning("Patient with Phone: {Phone} not found.", request.Phone);
                return NotFound<GetPatientDTO>();
            }

            var patientsMapper = mapper.Map<GetPatientDTO>(patient);

            logger.LogInformation("Successfully retrieved patient with Phone: {Phone}", request.Phone);
            return Success(patientsMapper);
        }
    }
}
