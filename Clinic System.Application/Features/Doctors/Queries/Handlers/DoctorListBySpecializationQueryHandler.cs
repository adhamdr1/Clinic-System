namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListBySpecializationQueryHandler : ResponseHandler, IRequestHandler<GetDoctorListBySpecializationQuery, Response<List<GetDoctorBasicInfoDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<DoctorListBySpecializationQueryHandler> logger;

        public DoctorListBySpecializationQueryHandler(IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorListBySpecializationQueryHandler> logger,
            ICacheService cacheService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public async Task<Response<List<GetDoctorBasicInfoDTO>>> Handle(GetDoctorListBySpecializationQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorListBySpecializationQuery for Specialization: {Specialization}", request.Specialization);

            // أ. بناء مفتاح مميز للصفحة دي تحديداً
            var cacheKey = $"DoctorListBySpecialization:{request.Specialization.Trim().ToLower()}";

            // ب. نسأل الـ Redis: "هل عندك الداتا دي؟"
            var cachedDoctors = await cacheService.GetDataAsync<List<GetDoctorBasicInfoDTO>>(cacheKey);

            // ج. لو الداتا موجودة في الكاش، هنرجعها فوراً ومش هنكمل باقي الكود (وفرنا رحلة للداتابيز)
            if (cachedDoctors != null)
            {
                logger.LogInformation("Successfully retrieved doctors from CACHE for {CacheKey}", cacheKey);
                return Success(cachedDoctors); // هنرجع نفس نوع الـ Response اللي الفرونت مستنيه
            }

            var doctors = await doctorService.GetDoctorsListBySpecializationAsync(request.Specialization, cancellationToken);

            if (doctors?.Any() != true)
            {
                logger.LogWarning("No doctors found for Specialization: {Specialization}", request.Specialization);
                return NotFound<List<GetDoctorBasicInfoDTO>>($"No doctors found with specialization: {request.Specialization}");
            }

            var doctorsMapper = mapper.Map<List<GetDoctorBasicInfoDTO>>(doctors);

            logger.LogInformation("Found {Count} doctors for Specialization: {Specialization}", doctorsMapper.Count, request.Specialization);

            await cacheService.SetDataAsync(cacheKey, doctorsMapper, TimeSpan.FromMinutes(60));

            return Success(doctorsMapper);
        }
    }
}
