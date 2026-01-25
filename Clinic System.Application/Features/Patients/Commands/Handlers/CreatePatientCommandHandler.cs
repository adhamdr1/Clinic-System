namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class CreatePatientCommandHandler : ResponseHandler, IRequestHandler<CreatePatientCommand, Response<CreatePatientDTO>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly ILogger<CreatePatientCommandHandler> logger;

        public CreatePatientCommandHandler(
            IPatientService patientService,
            IMapper mapper,
            IIdentityService identityService,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<CreatePatientCommandHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.logger = logger;
        }
        public async Task<Response<CreatePatientDTO>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            Patient patient = null;
            string userId = string.Empty;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting the process to add a new patient with name: {PatientName}", request.FullName);
                try
                {
                    userId = await identityService.CreateUserAsync(
                        request.UserName,
                        request.Email,
                        request.Password,
                        "Patient",
                        cancellationToken
                    );

                    patient = mapper.Map<Patient>(request);
                    patient.ApplicationUserId = userId;

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


            try
            {
                var token = await identityService.GenerateEmailConfirmationTokenAsync(userId);
                var encodedToken = identityService.EncodeToken(token);

                var confirmationLink = $"{request.BaseUrl}/api/authentication/confirm-email?UserId={userId}&Code={encodedToken}";

                var emailBody = EmailTemplates.GetEmailConfirmationTemplate(
                                    request.FullName,
                                    request.UserName,
                                    request.Email,
                                    confirmationLink
                                );

                // 4. الإرسال
                await emailService.SendEmailAsync(request.Email, "Welcome to Elite Clinic - Confirm Your Email", emailBody);

                logger.LogInformation("Confirmation email sent to {Email}", request.Email);
            }
            catch (Exception ex)
            {
                // لو فشل الإيميل مش بنوقف العملية، بس بنسجل تحذير
                logger.LogWarning(ex, "Patient created but failed to send confirmation email to {Email}", request.Email);
            }

            var patientsMapper = mapper.Map<CreatePatientDTO>(patient);

            patientsMapper.Email = request.Email;

            var locationUri = $"/api/patients/id/{patient.Id}";

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
  "email": "adhamdr32@gmail.com",
  "password": "Doma.dr1",
  "confirmPassword": "Doma.dr1"
}
 */
