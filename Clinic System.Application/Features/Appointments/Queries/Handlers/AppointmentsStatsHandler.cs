namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsStatsHandler : IRequestHandler<GetAdminAppointmentsStatsQuery, AppointmentStatsDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AppointmentsStatsHandler> logger;
        private readonly IAppointmentService appointmentService;

        public AppointmentsStatsHandler(
            IUnitOfWork unitOfWork,
            ILogger<AppointmentsStatsHandler> logger,
            IAppointmentService appointmentService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.appointmentService = appointmentService;
        }

        public async Task<AppointmentStatsDto> Handle(GetAdminAppointmentsStatsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetAdminAppointmentsStatsQuery");

            var stats = await appointmentService.GetAdminAppointmentsStatsAsync(request , cancellationToken);

            return stats;
        }
    }
}
