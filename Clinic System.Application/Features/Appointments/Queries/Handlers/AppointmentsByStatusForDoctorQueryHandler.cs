namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AppointmentsByStatusForDoctorQueryHandler : AppRequestHandler<GetAppointmentsByStatusForDoctorQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<AppointmentsByStatusForDoctorQueryHandler> logger;

        public AppointmentsByStatusForDoctorQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IMapper mapper,
            ILogger<AppointmentsByStatusForDoctorQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetAppointmentsByStatusForDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctorUserId = await doctorService.GetDoctorUserIdAsync(request.DoctorId, cancellationToken);

            if (doctorUserId == null)
                return NotFound<PagedResult<DoctorAppointmentDTO>>("Doctor not found");

            var authResult = await ValidateOwner(doctorUserId);
            if (authResult != null) return authResult;

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
