namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForDoctorQueryHandler : AppRequestHandler<GetAppointmentsByStatusForDoctorQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<AppointmentsByStatusForDoctorQueryHandler> logger;

        public AppointmentsByStatusForDoctorQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
           ILogger<AppointmentsByStatusForDoctorQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetAppointmentsByStatusForDoctorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAppointmentsByStatusForDoctor");

            var (authorizedId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

            if (errorResponse != null) return errorResponse;

            request.DoctorId = authorizedId;

            var DoctorwithAppointment = await appointmentService.GetAppointmentsByStatusForDoctorAsync(request, cancellationToken);

            var DoctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(DoctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(DoctorwithAppointmentmapper, DoctorwithAppointment.TotalCount,
                DoctorwithAppointment.CurrentPage, DoctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", DoctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
