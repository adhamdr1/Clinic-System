namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PastAppointmentsForPatientQueryHandler : ResponseHandler, IRequestHandler<GetPastAppointmentsForPatientQuery, Response<PagedResult<PatientAppointmentDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<PastAppointmentsForPatientQueryHandler> logger;

        public PastAppointmentsForPatientQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<PastAppointmentsForPatientQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<PatientAppointmentDTO>>> Handle(GetPastAppointmentsForPatientQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetPastAppointmentsForPatient for PatientId={PatientId}", request.PatientId);

            var PatientwithAppointment = await appointmentService.GetPastAppointmentsForPatientAsync(request);

            var PatientwithAppointmentmapper = mapper.Map<List<PatientAppointmentDTO>>(PatientwithAppointment.Items);

            var pagedResult = new PagedResult<PatientAppointmentDTO>(PatientwithAppointmentmapper, PatientwithAppointment.TotalCount,
                PatientwithAppointment.CurrentPage, PatientwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", PatientwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
