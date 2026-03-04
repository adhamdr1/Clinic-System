namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PastAppointmentsForDoctorQueryHandler : AppRequestHandler<GetPastAppointmentsForDoctorQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<PastAppointmentsForDoctorQueryHandler> logger;

        public PastAppointmentsForDoctorQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            ICacheService cacheService,
           ILogger<PastAppointmentsForDoctorQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetPastAppointmentsForDoctorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetPastAppointmentsForDoctor for DoctorId={DoctorId}", request.DoctorId);

            var (authorizedId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

            if (errorResponse != null) return errorResponse;

            request.DoctorId = authorizedId;

            // 2. بناء مفتاح الكاش (شامل رقم الصفحة والحجم)
            string cacheKey = $"PastAppts_Doctor_{request.DoctorId}_Page_{request.PageNumber}_Size_{request.PageSize}";

            var cachedResult = await cacheService.GetDataAsync<PagedResult<DoctorAppointmentDTO>>(cacheKey);
            if (cachedResult != null)
            {
                logger.LogInformation("Retrieved Past Appointments from CACHE for Doctor {DoctorId}", request.DoctorId);
                return Success(cachedResult);
            }

            var doctorwithAppointment = await appointmentService.GetPastAppointmentsForDoctorAsync(request);

            var doctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(doctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(doctorwithAppointmentmapper, doctorwithAppointment.TotalCount,
                doctorwithAppointment.CurrentPage, doctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", doctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            // 3. حفظ في الكاش لمدة ساعة (60 دقيقة)
            await cacheService.SetDataAsync(cacheKey, pagedResult, TimeSpan.FromMinutes(60));

            return Success(pagedResult);
        }
    }
}
