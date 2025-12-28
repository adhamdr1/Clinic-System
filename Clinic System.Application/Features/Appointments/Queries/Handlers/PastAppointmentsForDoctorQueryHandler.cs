namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PastAppointmentsForDoctorQueryHandler : ResponseHandler, IRequestHandler<GetPastAppointmentsForDoctorQuery, Response<PagedResult<DoctorAppointmentDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<PastAppointmentsForDoctorQueryHandler> logger;

        public PastAppointmentsForDoctorQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<PastAppointmentsForDoctorQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetPastAppointmentsForDoctorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetPastAppointmentsForDoctor for DoctorId={DoctorId}", request.DoctorId);

            var doctorwithAppointment = await appointmentService.GetPastAppointmentsForDoctorAsync(request);

            var doctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(doctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(doctorwithAppointmentmapper, doctorwithAppointment.TotalCount,
                doctorwithAppointment.CurrentPage, doctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", doctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
