namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PatientAppointmentsQueryHandler : ResponseHandler, IRequestHandler<GetPatientAppointmentsQuery, Response<PagedResult<PatientAppointmentDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientAppointmentsQueryHandler> logger;

        public PatientAppointmentsQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<PatientAppointmentsQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<PatientAppointmentDTO>>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetPatientAppointments for PatientId={PatientId}", request.patientId);

            if (request.pageNumber < 1)
            {
                logger.LogWarning("Invalid PageNumber={PageNumber} requested", request.pageNumber);
                return BadRequest<PagedResult<PatientAppointmentDTO>>("Page number must be greater than 0");
            }

            if (request.pageSize < 1 || request.pageSize > 100)
            {
                logger.LogWarning("Invalid PageSize={PageSize} requested", request.pageSize);
                return BadRequest<PagedResult<PatientAppointmentDTO>>("Page size must be between 1 and 100");
            }

            if (request.patientId < 1)
            {
                logger.LogWarning("Invalid PatientId={PatientId} requested", request.patientId);
                return BadRequest<PagedResult<PatientAppointmentDTO>>("PatientId number must be greater than 0");
            }

            var PatientwithAppointment = await appointmentService.GetPatientAppointmentsAsync(request);

            var PatientwithAppointmentmapper = mapper.Map<List<PatientAppointmentDTO>>(PatientwithAppointment.Items);

            var pagedResult = new PagedResult<PatientAppointmentDTO>(PatientwithAppointmentmapper, PatientwithAppointment.TotalCount,
                PatientwithAppointment.CurrentPage, PatientwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", PatientwithAppointment.Items.Count(), request.pageNumber, request.pageSize);

            return Success(pagedResult);
        }
    }
}
