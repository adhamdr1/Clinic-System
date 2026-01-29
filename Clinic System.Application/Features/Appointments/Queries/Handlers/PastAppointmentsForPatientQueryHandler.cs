namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PastAppointmentsForPatientQueryHandler : AppRequestHandler<GetPastAppointmentsForPatientQuery, PagedResult<PatientAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<PastAppointmentsForPatientQueryHandler> logger;

        public PastAppointmentsForPatientQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<PastAppointmentsForPatientQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<PatientAppointmentDTO>>> Handle(GetPastAppointmentsForPatientQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetPastAppointmentsForPatient for PatientId={PatientId}", request.PatientId);

            var (authorizedId, errorResponse) = await GetAuthorizedPatientId(request.PatientId);

            if (errorResponse != null) return errorResponse;

            request.PatientId = authorizedId;

            var PatientwithAppointment = await appointmentService.GetPastAppointmentsForPatientAsync(request);

            var PatientwithAppointmentmapper = mapper.Map<List<PatientAppointmentDTO>>(PatientwithAppointment.Items);

            var pagedResult = new PagedResult<PatientAppointmentDTO>(PatientwithAppointmentmapper, PatientwithAppointment.TotalCount,
                PatientwithAppointment.CurrentPage, PatientwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", PatientwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
