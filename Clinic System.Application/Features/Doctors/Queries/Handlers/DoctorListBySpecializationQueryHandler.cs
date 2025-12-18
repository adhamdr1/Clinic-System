namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListBySpecializationQueryHandler : ResponseHandler, IRequestHandler<GetDoctorListBySpecializationQuery, Response<List<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorListBySpecializationQueryHandler> logger;

        public DoctorListBySpecializationQueryHandler(IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorListBySpecializationQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<List<GetDoctorListDTO>>> Handle(GetDoctorListBySpecializationQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorListBySpecializationQuery for Specialization: {Specialization}", request.Specialization);

            var doctors = await doctorService.GetDoctorsListBySpecializationAsync(request.Specialization, cancellationToken);

            if (doctors?.Any() != true)
            {
                logger.LogWarning("No doctors found for Specialization: {Specialization}", request.Specialization);
                return NotFound<List<GetDoctorListDTO>>($"No doctors found with specialization: {request.Specialization}");
            }

            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors);

            logger.LogInformation("Found {Count} doctors for Specialization: {Specialization}", doctorsMapper.Count, request.Specialization);
            return Success(doctorsMapper);
        }
    }
}
