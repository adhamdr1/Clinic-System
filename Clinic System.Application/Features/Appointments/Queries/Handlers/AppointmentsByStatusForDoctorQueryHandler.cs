namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForDoctorQueryHandler : ResponseHandler, IRequestHandler<GetAppointmentsByStatusForDoctorQuery, Response<PagedResult<DoctorAppointmentDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<AppointmentsByStatusForDoctorQueryHandler> logger;

        public AppointmentsByStatusForDoctorQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<AppointmentsByStatusForDoctorQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetAppointmentsByStatusForDoctorQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAppointmentsByStatusForDoctor");

            var AdminwithAppointment = await appointmentService.GetAppointmentsByStatusForDoctorAsync(request, cancellationToken);

            var AdminwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(AdminwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(AdminwithAppointmentmapper, AdminwithAppointment.TotalCount,
                AdminwithAppointment.CurrentPage, AdminwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", AdminwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
