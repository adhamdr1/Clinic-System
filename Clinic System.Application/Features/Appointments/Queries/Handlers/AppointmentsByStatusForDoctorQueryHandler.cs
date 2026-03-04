namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForDoctorQueryHandler : AppRequestHandler<GetAppointmentsByStatusForDoctorQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<AppointmentsByStatusForDoctorQueryHandler> logger;

        public AppointmentsByStatusForDoctorQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            ICacheService cacheService,
           ILogger<AppointmentsByStatusForDoctorQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetAppointmentsByStatusForDoctorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAppointmentsByStatusForDoctor");

            var (authorizedId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

            if (errorResponse != null) return errorResponse;

            request.DoctorId = authorizedId;

            // مفتاح بيشمل الدكتور والحالة (مثلاً: دكتور 5 بيدور على المواعيد الـ Pending)
            string cacheKey = $"DoctorApptsByStatus_{request.DoctorId}_{request.Status}_Page_{request.PageNumber}_Size_{request.PageSize}";

            var cachedResult = await cacheService.GetDataAsync<PagedResult<DoctorAppointmentDTO>>(cacheKey);
            if (cachedResult != null) return Success(cachedResult);

            var DoctorwithAppointment = await appointmentService.GetAppointmentsByStatusForDoctorAsync(request, cancellationToken);

            var DoctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(DoctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(DoctorwithAppointmentmapper, DoctorwithAppointment.TotalCount,
                DoctorwithAppointment.CurrentPage, DoctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", DoctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            await cacheService.SetDataAsync(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return Success(pagedResult);
        }
    }
}
