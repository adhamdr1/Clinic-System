namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForAdminQueryHandler : ResponseHandler, IRequestHandler<GetAppointmentsByStatusForAdminQuery, Response<PagedResult<AppointmentsByStatusForAdminDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<AppointmentsByStatusForAdminQueryHandler> logger;

        public AppointmentsByStatusForAdminQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<AppointmentsByStatusForAdminQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<AppointmentsByStatusForAdminDTO>>> Handle(GetAppointmentsByStatusForAdminQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAppointmentsByStatusForAdmin");

            // مفتاح بيشمل الـ Status اللي الأدمن بيدور عليها
            string cacheKey = $"AdminApptsByStatus_{request.Status}_Page_{request.PageNumber}_Size_{request.PageSize}";

            var cachedResult = await cacheService.GetDataAsync<PagedResult<AppointmentsByStatusForAdminDTO>>(cacheKey);
            if (cachedResult != null)
                return Success(cachedResult);

            var AdminwithAppointment = await appointmentService.GetAppointmentsByStatusForAdminAsync(request,cancellationToken);

            var AdminwithAppointmentmapper = mapper.Map<List<AppointmentsByStatusForAdminDTO>>(AdminwithAppointment.Items);

            var pagedResult = new PagedResult<AppointmentsByStatusForAdminDTO>(AdminwithAppointmentmapper, AdminwithAppointment.TotalCount,
                AdminwithAppointment.CurrentPage, AdminwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", AdminwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            await cacheService.SetDataAsync(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return Success(pagedResult);
        }
    }
}
