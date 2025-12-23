namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class CreateDoctorCommandHandler : ResponseHandler, IRequestHandler<CreateDoctorCommand, Response<CreateDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateDoctorCommandHandler> logger;

        public CreateDoctorCommandHandler(IDoctorService doctorService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork , ILogger<CreateDoctorCommandHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<CreateDoctorDTO>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = null;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting the process to add a new doctor with name: {DoctorName}", request.FullName);
                try
                {
                    var UserId = await identityService.CreateUserAsync(
                        request.UserName,
                        request.Email,
                        request.Password,
                        "Doctor",
                        cancellationToken
                    );

                    doctor = mapper.Map<Doctor>(request);
                    doctor.ApplicationUserId = UserId;

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

            var doctorsMapper = mapper.Map<CreateDoctorDTO>(doctor);

            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                doctorsMapper.Email = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken) ?? string.Empty;

                if (string.IsNullOrEmpty(doctorsMapper.Email))
                {
                    // نستخدم Warning هنا لأن الموقف غريب (يوجد ID ولا يوجد ايميل) لكنه لا يعطل البرنامج
                    logger.LogWarning("Email not found for User ID: {UserId} during doctor mapping.", doctor.ApplicationUserId);
                }
            }

            var locationUri = $"/api/GetDoctorById/{doctor.Id}";

            logger.LogInformation("Doctor {DoctorName} added successfully with ID: {DoctorId}", request.FullName, doctor.Id);
            return Created<CreateDoctorDTO>(doctorsMapper, locationUri, "Doctor created successfully");
        }
    }
}
/*
 {
  "fullName": "Dr.Nour Farag",
  "gender": "female",
  "dateOfBirth": "1979-07-22",
  "phone": "01070689484",
  "address": "Alex",
  "specialization": "string",
  "userName": "Nourdr7",
  "email": "Nour7@g.c",
  "password": "Doma.dr1",
  "confirmPassword": "Doma.dr1"
}
 */