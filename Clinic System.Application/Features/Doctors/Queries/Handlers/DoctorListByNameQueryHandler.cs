namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListByNameQueryHandler : ResponseHandler, IRequestHandler<GetDoctorListByNameQuery, Response<List<GetDoctorBasicInfoDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorListByNameQueryHandler> logger;

        public DoctorListByNameQueryHandler(IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorListByNameQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<List<GetDoctorBasicInfoDTO>>> Handle(GetDoctorListByNameQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorListByNameQuery for Name: {Name}", request.FullName);

            var doctors = await doctorService.GetDoctorsListByNameAsync(request.FullName, cancellationToken);

            if (doctors?.Any() != true)
            {
                logger.LogWarning("No doctors found for Name: {Name}", request.FullName);
                return NotFound<List<GetDoctorBasicInfoDTO>>($"No doctors found with Name: {request.FullName}");
            }

            var doctorsMapper = mapper.Map<List<GetDoctorBasicInfoDTO>>(doctors);

            logger.LogInformation("Found {Count} doctors for Name: {Name}", doctorsMapper.Count, request.FullName);
            return Success(doctorsMapper);
        }
    }
}
