namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class UpdateDoctorCommandHandler : AppRequestHandler<UpdateDoctorCommand, UpdateDoctorDTO>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdateDoctorCommandHandler> logger;

        public UpdateDoctorCommandHandler(IDoctorService doctorService, ICurrentUserService currentUserService
            , IMapper mapper, IUnitOfWork unitOfWork, ILogger<UpdateDoctorCommandHandler> logger) : base(currentUserService) //
        {  
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<UpdateDoctorDTO>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting update process for doctor profile with Id {DoctorId}.", request.Id);

            var authResult = await ValidateDoctorAccess(request.Id);
            if (authResult != null)
                return authResult;

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with Id {DoctorId} not found.", request.Id);
                return NotFound<UpdateDoctorDTO>($"Doctor with Id {request.Id} not found");
            }

            mapper.Map(request, doctor);

            await doctorService.UpdateDoctor(doctor, cancellationToken);

            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                logger.LogError("Failed to update doctor profile with Id {DoctorId} in the database.", request.Id);
                return BadRequest<UpdateDoctorDTO>("Failed to update doctor profile in the database.");
            }

            var doctorsMapper = mapper.Map<UpdateDoctorDTO>(doctor);

            logger.LogInformation("Doctor profile with Id {DoctorId} updated successfully.", request.Id);
            return Success<UpdateDoctorDTO>(doctorsMapper, "Doctor updated successfully");
        }
    }
}