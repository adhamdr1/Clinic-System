namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorByIdQueryHandler : ResponseHandler, IRequestHandler<GetDoctorByIdQuery, Response<GetDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorByIdQueryHandler(
            IDoctorService doctorService,
            IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<Response<GetDoctorDTO>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id < 1)
                return BadRequest<GetDoctorDTO>("ID must be greater than 0");

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id, cancellationToken);
            if (doctor == null)
                return NotFound<GetDoctorDTO>();

            var doctorsMapper = mapper.Map<GetDoctorDTO>(doctor);

            return Success(doctorsMapper);
        }
    }
}
