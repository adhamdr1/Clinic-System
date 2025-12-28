namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForAdminQueryHandler : ResponseHandler, IRequestHandler<GetAppointmentsByStatusForAdminQuery, Response<PagedResult<AppointmentsByStatusForAdminDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<AppointmentsByStatusForAdminQueryHandler> logger;

        public AppointmentsByStatusForAdminQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<AppointmentsByStatusForAdminQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<AppointmentsByStatusForAdminDTO>>> Handle(GetAppointmentsByStatusForAdminQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetAppointmentsByStatusForAdmin");

            var AdminwithAppointment = await appointmentService.GetAppointmentsByStatusForAdminAsync(request,cancellationToken);

            var AdminwithAppointmentmapper = mapper.Map<List<AppointmentsByStatusForAdminDTO>>(AdminwithAppointment.Items);

            var pagedResult = new PagedResult<AppointmentsByStatusForAdminDTO>(AdminwithAppointmentmapper, AdminwithAppointment.TotalCount,
                AdminwithAppointment.CurrentPage, AdminwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", AdminwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
