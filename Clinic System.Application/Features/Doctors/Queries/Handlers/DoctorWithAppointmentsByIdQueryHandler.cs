namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorWithAppointmentsByIdQueryHandler : AppRequestHandler<GetDoctorWithAppointmentsByIdQuery, GetDoctorWhitAppointmentDTO>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ILogger<DoctorWithAppointmentsByIdQueryHandler> logger;

        public DoctorWithAppointmentsByIdQueryHandler(
            ICurrentUserService currentUserService,
            IDoctorService doctorService,
            IMapper mapper,
            IIdentityService identityService,
            ILogger<DoctorWithAppointmentsByIdQueryHandler> logger) : base(currentUserService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.logger = logger;
        }

        public override async Task<Response<GetDoctorWhitAppointmentDTO>> Handle(GetDoctorWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var authResult = await ValidateDoctorAccess(request.Id);
            if (authResult != null)
                return authResult;

            var doctor = await doctorService.GetDoctorWithAppointmentsByIdAsync(request.Id, cancellationToken);

            if (doctor == null)
            {
                logger.LogInformation("GetDoctorWithAppointmentsByIdQueryHandler: Doctor with ID {Id} not found", request.Id);
                return NotFound<GetDoctorWhitAppointmentDTO>($"Doctor with ID {request.Id} not found");
            }

            var doctorsMapper = mapper.Map<GetDoctorWhitAppointmentDTO>(doctor);

            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                var (email, userName) = await identityService.GetUserEmailAndUserNameAsync(doctor.ApplicationUserId, cancellationToken);

                doctorsMapper.Email = email;
                doctorsMapper.UserName = userName;

                if (string.IsNullOrEmpty(doctorsMapper.Email) || string.IsNullOrEmpty(doctorsMapper.UserName))
                {
                    logger.LogWarning("Missing Identity data for Doctor AppUserId: {AppUserId}", doctor.ApplicationUserId);
                }
            }

            logger.LogInformation("Successfully retrieved doctor with appointments, ID: {Id}", request.Id);

            return Success(doctorsMapper);
        }
    }
}
