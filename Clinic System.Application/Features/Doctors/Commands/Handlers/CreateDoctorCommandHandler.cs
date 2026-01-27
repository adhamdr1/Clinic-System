namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class CreateDoctorCommandHandler : ResponseHandler, IRequestHandler<CreateDoctorCommand, Response<CreateDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IEmailService emailService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateDoctorCommandHandler> logger;

        public CreateDoctorCommandHandler(
            IDoctorService doctorService,
            IMapper mapper,
            IIdentityService identityService,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ILogger<CreateDoctorCommandHandler> logger
        )
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.emailService = emailService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<CreateDoctorDTO>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = null;
            string userId = string.Empty;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting the process to add a new doctor with name: {DoctorName}", request.FullName);
                try
                {
                    userId = await identityService.CreateUserAsync(
                        request.UserName,
                        request.Email,
                        request.Password,
                        "Doctor",
                        cancellationToken
                    );

                    doctor = mapper.Map<Doctor>(request);
                    doctor.ApplicationUserId = userId;

                    await doctorService.CreateDoctorAsync(doctor, cancellationToken);
                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogWarning("Failed to save the doctor {DoctorName} to the database", request.FullName);
                        return BadRequest<CreateDoctorDTO>("Failed to create doctor");
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while adding doctor: {DoctorName}", request.FullName);
                    return BadRequest<CreateDoctorDTO>($"User creation failed: {ex.Message}");
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
                                    confirmationLink,
                                    request.Specialization
                                );

                // 4. الإرسال
                await emailService.SendEmailAsync(request.Email, "Welcome to Elite Clinic - Confirm Your Email", emailBody);

                logger.LogInformation("Confirmation email sent to {Email}", request.Email);
            }
            catch (Exception ex)
            {
                // لو فشل الإيميل مش بنوقف العملية، بس بنسجل تحذير
                logger.LogWarning(ex, "Doctor created but failed to send confirmation email to {Email}", request.Email);
            }

            var doctorsMapper = mapper.Map<CreateDoctorDTO>(doctor);

            doctorsMapper.Email = request.Email;

            var locationUri = $"/api/doctors/id/{doctor.Id}";

            logger.LogInformation("Doctor {DoctorName} added successfully with ID: {DoctorId}", request.FullName, doctor.Id);
            return Created<CreateDoctorDTO>(doctorsMapper, locationUri, "Doctor created successfully");
        }
    }
}
/*
 {
  "fullName": "Nour Farag",
  "gender": "female",
  "dateOfBirth": "1979-07-22",
  "phone": "01070689484",
  "address": "Alex",
  "specialization": "string",
  "userName": "Nourdr7",
  "email": "adhamdr32@gmail.com",
  "password": "Doma.dr1",
  "confirmPassword": "Doma.dr1"
}
 */