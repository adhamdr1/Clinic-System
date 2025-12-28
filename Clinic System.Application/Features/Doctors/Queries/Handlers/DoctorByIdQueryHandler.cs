namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorByIdQueryHandler : ResponseHandler, IRequestHandler<GetDoctorByIdQuery, Response<GetDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorByIdQueryHandler> logger;

        public DoctorByIdQueryHandler(
            IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorByIdQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<GetDoctorDTO>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorByIdQuery for ID: {Id}", request.Id);

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id, cancellationToken);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with ID: {Id} not found.", request.Id);
                return NotFound<GetDoctorDTO>();
            }

            var doctorsMapper = mapper.Map<GetDoctorDTO>(doctor);

            logger.LogInformation("Successfully retrieved doctor with ID: {Id}", request.Id);
            return Success(doctorsMapper);
        }
    }
}
