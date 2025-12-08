namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorWithAppointmentsByIdQueryHandler : ResponseHandler, IRequestHandler<GetDoctorWithAppointmentsByIdQuery, Response<GetDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;

        public DoctorWithAppointmentsByIdQueryHandler(
            IDoctorService doctorService, 
            IMapper mapper,
            IIdentityService identityService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
        }

        public async Task<Response<GetDoctorDTO>> Handle(GetDoctorWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id < 1)
                return BadRequest<GetDoctorDTO>("ID must be greater than 0");

            var doctor = await doctorService.GetDoctorWithAppointmentsByIdAsync(request.Id, cancellationToken);
            if (doctor == null)
                return NotFound<GetDoctorDTO>();

            var doctorsMapper = mapper.Map<GetDoctorDTO>(doctor);

            // Get Email from UserService using ApplicationUserId
            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                doctorsMapper.Email = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken) ?? string.Empty;
            }

            return Success(doctorsMapper);
        }
    }
}
