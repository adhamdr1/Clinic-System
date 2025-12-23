namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class CreatePatientCommandHandler : ResponseHandler, IRequestHandler<CreatePatientCommand, Response<CreatePatientDTO>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreatePatientCommandHandler> logger;

        public CreatePatientCommandHandler(IPatientService patientService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork , ILogger<CreatePatientCommandHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<CreatePatientDTO>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            Patient patient = null;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting the process to add a new patient with name: {PatientName}", request.FullName);
                try
                {
                    var UserId = await identityService.CreateUserAsync(
                        request.UserName,
                        request.Email,
                        request.Password,
                        "Patient",
                        cancellationToken
                    );

                    patient = mapper.Map<Patient>(request);
                    patient.ApplicationUserId = UserId;

                    await patientService.CreatePatientAsync(patient, cancellationToken);
                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogWarning("Failed to save the patient {PatientName} to the database", request.FullName);
                        return BadRequest<CreatePatientDTO>("Failed to create patient");
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while adding patient: {PatientName}", request.FullName);
                    return BadRequest<CreatePatientDTO>($"User creation failed: {ex.Message}");
                }
            }

            var patientsMapper = mapper.Map<CreatePatientDTO>(patient);

            if (!string.IsNullOrEmpty(patient.ApplicationUserId))
            {
                patientsMapper.Email = await identityService.GetUserEmailAsync(patient.ApplicationUserId, cancellationToken) ?? string.Empty;

                if (string.IsNullOrEmpty(patientsMapper.Email))
                {
                    // نستخدم Warning هنا لأن الموقف غريب (يوجد ID ولا يوجد ايميل) لكنه لا يعطل البرنامج
                    logger.LogWarning("Email not found for User ID: {UserId} during patient mapping.", patient.ApplicationUserId);
                }
            }

            var locationUri = $"/api/GetPatientById/{patient.Id}";

            logger.LogInformation("Patient {PatientName} added successfully with ID: {PatientId}", request.FullName, patient.Id);
            return Created<CreatePatientDTO>(patientsMapper, locationUri, "Patient created successfully");
        }
    }
}
/*
 {
  "fullName": "Nour Farag",
  "gender": "female",
  "dateOfBirth": "1979-07-22",
  "phone": "01000689484",
  "address": "Alex",
  "userName": "Nourdr1",
  "email": "Nour@g.c",
  "password": "Doma.dr1",
  "confirmPassword": "Doma.dr1"
}
 */
