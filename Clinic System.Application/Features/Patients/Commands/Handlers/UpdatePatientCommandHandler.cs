namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class UpdatePatientCommandHandler : AppRequestHandler<UpdatePatientCommand, UpdatePatientDTO>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdatePatientCommandHandler> logger;

        public UpdatePatientCommandHandler(
           ICurrentUserService currentUserService, // <--- الجديد
           IPatientService patientService,
           IMapper mapper,
           IUnitOfWork unitOfWork,
           ILogger<UpdatePatientCommandHandler> logger) : base(currentUserService)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<UpdatePatientDTO>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting update process for Patient profile with Id {PatientId}.", request.Id);
            
            var authResult = await ValidatePatientAccess(request.Id);
            if (authResult != null)
                return authResult;


            var patient = await patientService.GetPatientByIdAsync(request.Id);

            if (patient == null)
            {
                logger.LogWarning("Patient with Id {PatientId} not found.", request.Id);
                return NotFound<UpdatePatientDTO>($"Patient with Id {request.Id} not found");
            }

            mapper.Map(request, patient);

            await patientService.UpdatePatient(patient, cancellationToken);

            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                logger.LogError("Failed to update Patient profile with Id {PatientId} in the database.", request.Id);
                return BadRequest<UpdatePatientDTO>("Failed to update Patient profile in the database.");
            }

            var patientsMapper = mapper.Map<UpdatePatientDTO>(patient);

            logger.LogInformation("Patient profile with Id {PatientId} updated successfully.", request.Id);
            return Success<UpdatePatientDTO>(patientsMapper, "Patient updated successfully");
        }
    }
}