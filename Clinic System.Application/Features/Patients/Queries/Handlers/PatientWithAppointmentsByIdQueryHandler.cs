using Clinic_System.Core.Entities;

namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientWithAppointmentsByIdQueryHandler : ResponseHandler, IRequestHandler<GetPatientWithAppointmentsByIdQuery, Response<GetPatientWhitAppointmentDTO>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ILogger<PatientWithAppointmentsByIdQueryHandler> logger;

        public PatientWithAppointmentsByIdQueryHandler(
            IPatientService patientService,
            IMapper mapper,
            IIdentityService identityService,
            ILogger<PatientWithAppointmentsByIdQueryHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async Task<Response<GetPatientWhitAppointmentDTO>> Handle(GetPatientWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {
            
            var patient = await patientService.GetPatientWithAppointmentsByIdAsync(request.Id, cancellationToken);

            if (patient == null)
            {
                logger.LogInformation("GetPatientWithAppointmentsByIdQueryHandler: Patient with ID {Id} not found", request.Id);
                return NotFound<GetPatientWhitAppointmentDTO>($"Patient with ID {request.Id} not found");
            }

            var patientsMapper = mapper.Map<GetPatientWhitAppointmentDTO>(patient);

            // Get Email from UserService using ApplicationUserId
            if (!string.IsNullOrEmpty(patient.ApplicationUserId))
            {
                patientsMapper.Email = await identityService.GetUserEmailAsync(patient.ApplicationUserId, cancellationToken) ?? string.Empty;

                if (string.IsNullOrEmpty(patientsMapper.Email))
                {
                    logger.LogWarning("GetPatientWithAppointmentsByIdQueryHandler: Email not found for ApplicationUserId {ApplicationUserId}", patient.ApplicationUserId);
                }
            }

            // Get UserName from UserService using ApplicationUserId
            if (!string.IsNullOrEmpty(patient.ApplicationUserId))
            {
                patientsMapper.UserName = await identityService.GetUserNameAsync(patient.ApplicationUserId, cancellationToken) ?? string.Empty;

                if (string.IsNullOrEmpty(patientsMapper.UserName))
                {
                    logger.LogWarning("GetPatientWithAppointmentsByIdQueryHandler: UserName not found for ApplicationUserId {ApplicationUserId}", patient.ApplicationUserId);
                }
            }

            logger.LogInformation("GetPatientWithAppointmentsByIdQueryHandler: Successfully retrieved patient with ID {Id}", request.Id);

            return Success(patientsMapper);
        }
    }
}
