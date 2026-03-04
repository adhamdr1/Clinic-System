namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsStatsHandler : IRequestHandler<GetAdminAppointmentsStatsQuery, AppointmentStatsDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AppointmentsStatsHandler> logger;
        private readonly IAppointmentService appointmentService;
        private readonly ICacheService cacheService;

        public AppointmentsStatsHandler(
            IUnitOfWork unitOfWork,
            ILogger<AppointmentsStatsHandler> logger,
            IAppointmentService appointmentService,
            ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.appointmentService = appointmentService;
            this.cacheService = cacheService;
        }

        public async Task<AppointmentStatsDto> Handle(GetAdminAppointmentsStatsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetAdminAppointmentsStatsQuery");

            string startStr = request.StartDate?.ToString("yyyyMMdd") ?? "AllTime";
            string endStr = request.EndDate?.ToString("yyyyMMdd") ?? "AllTime";
            string cacheKey = $"AdminStats_Start_{startStr}_End_{endStr}";

            var cachedStats = await cacheService.GetDataAsync<AppointmentStatsDto>(cacheKey);

            if (cachedStats != null)
            {
                logger.LogInformation("Successfully retrieved Admin Stats from CACHE for {CacheKey}", cacheKey);
                return cachedStats; // رجعها فوراً ووفر على الـ SQL الحسابات التقيلة
            }

            var stats = await appointmentService.GetAdminAppointmentsStatsAsync(request, cancellationToken);

            if (stats != null)
            {
                // هنخلي الإحصائيات تعيش 30 دقيقة، ده وقت ممتاز جداً للأدمن
                await cacheService.SetDataAsync(cacheKey, stats, TimeSpan.FromMinutes(30));
                logger.LogInformation("Saved new Admin Stats to CACHE for {CacheKey} (TTL: 30 mins)", cacheKey);
            }

            return stats;
        }
    }
}
