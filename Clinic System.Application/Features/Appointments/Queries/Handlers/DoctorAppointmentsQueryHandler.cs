namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class DoctorAppointmentsQueryHandler : AppRequestHandler<GetDoctorAppointmentsQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorAppointmentsQueryHandler> logger;

        public DoctorAppointmentsQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<DoctorAppointmentsQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetDoctorAppointments for DoctorId={DoctorId}", request.DoctorId);

            var (authorizedId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

            if (errorResponse != null) return errorResponse;

            request.DoctorId = authorizedId;

            var doctorwithAppointment = await appointmentService.GetDoctorAppointmentsAsync(request);
           
            var doctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(doctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(doctorwithAppointmentmapper, doctorwithAppointment.TotalCount,
                doctorwithAppointment.CurrentPage, doctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", doctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
