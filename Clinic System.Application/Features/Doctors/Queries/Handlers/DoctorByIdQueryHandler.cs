namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorByIdQueryHandler : AppRequestHandler<GetDoctorByIdQuery, GetDoctorDTO>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<DoctorByIdQueryHandler> logger;

        public DoctorByIdQueryHandler(ICurrentUserService currentUserService,
            IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorByIdQueryHandler> logger,
            ICacheService cacheService) : base(currentUserService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public override async Task<Response<GetDoctorDTO>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorByIdQuery for ID: {Id}", request.Id);


            var authResult = await ValidateDoctorAccess(request.Id);
            if (authResult != null)
                return authResult;

            // أ. بناء مفتاح مميز للصفحة دي تحديداً
            string cacheKey = $"DoctorProfile_{request.Id}";

            // ب. نسأل الـ Redis: "هل عندك الداتا دي؟"
            var cachedDoctor = await cacheService.GetDataAsync<GetDoctorDTO>(cacheKey);

            // ج. لو الداتا موجودة في الكاش، هنرجعها فوراً ومش هنكمل باقي الكود (وفرنا رحلة للداتابيز)
            if (cachedDoctor != null)
            {
                logger.LogInformation("Successfully retrieved doctor from CACHE for {CacheKey}", cacheKey);
                return Success(cachedDoctor); // هنرجع نفس نوع الـ Response اللي الفرونت مستنيه
            }

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id, cancellationToken);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with ID: {Id} not found.", request.Id);
                return NotFound<GetDoctorDTO>();
            }

            var doctorsMapper = mapper.Map<GetDoctorDTO>(doctor);

            logger.LogInformation("Successfully retrieved doctor with ID: {Id}", request.Id);

            await cacheService.SetDataAsync(cacheKey, doctorsMapper, TimeSpan.FromMinutes(30));

            return Success(doctorsMapper);
        }
    }
}
