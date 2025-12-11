namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class CreateDoctorCommandHandler : ResponseHandler, IRequestHandler<CreateDoctorCommand, Response<CreateDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;

        public CreateDoctorCommandHandler(IDoctorService doctorService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<CreateDoctorDTO>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = null;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {   
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
                        return BadRequest<CreateDoctorDTO>("Failed to create doctor");
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return BadRequest<CreateDoctorDTO>($"User creation failed: {ex.Message}");
                }
            }

            var doctorsMapper = mapper.Map<CreateDoctorDTO>(doctor);

            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                doctorsMapper.Email = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken) ?? string.Empty;
            }

            var locationUri = $"/api/GetDoctorById/{doctor.Id}";

            return Created<CreateDoctorDTO>(doctorsMapper, locationUri, "Doctor created successfully");
        }
    }
}
