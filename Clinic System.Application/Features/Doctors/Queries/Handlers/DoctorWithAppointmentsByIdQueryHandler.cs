namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorWithAppointmentsByIdQueryHandler : ResponseHandler, IRequestHandler<GetDoctorWithAppointmentsByIdQuery, Response<GetDoctorWhitAppointmentDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ILogger<DoctorWithAppointmentsByIdQueryHandler> logger;

        public DoctorWithAppointmentsByIdQueryHandler(
            IDoctorService doctorService, 
            IMapper mapper,
            IIdentityService identityService,
            ILogger<DoctorWithAppointmentsByIdQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async Task<Response<GetDoctorWhitAppointmentDTO>> Handle(GetDoctorWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await doctorService.GetDoctorWithAppointmentsByIdAsync(request.Id, cancellationToken);

            if (doctor == null)
            {
                logger.LogInformation("GetDoctorWithAppointmentsByIdQueryHandler: Doctor with ID {Id} not found", request.Id);
                return NotFound<GetDoctorWhitAppointmentDTO>($"Doctor with ID {request.Id} not found");
            }

            var doctorsMapper = mapper.Map<GetDoctorWhitAppointmentDTO>(doctor);

            // Get Email from UserService using ApplicationUserId
            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                doctorsMapper.Email = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken) ?? string.Empty;
            
                if (string.IsNullOrEmpty(doctorsMapper.Email))
                {
                    logger.LogWarning("GetDoctorWithAppointmentsByIdQueryHandler: Email not found for ApplicationUserId {ApplicationUserId}", doctor.ApplicationUserId);
                }
            }

            // Get UserName from UserService using ApplicationUserId
            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                doctorsMapper.UserName = await identityService.GetUserNameAsync(doctor.ApplicationUserId, cancellationToken) ?? string.Empty;

                if (string.IsNullOrEmpty(doctorsMapper.UserName))
                {
                    logger.LogWarning("GetDoctorWithAppointmentsByIdQueryHandler: UserName not found for ApplicationUserId {ApplicationUserId}", doctor.ApplicationUserId);
                }
            }

            logger.LogInformation("GetDoctorWithAppointmentsByIdQueryHandler: Successfully retrieved doctor with ID {Id}", request.Id);
            
            return Success(doctorsMapper);
        }
    }
}
