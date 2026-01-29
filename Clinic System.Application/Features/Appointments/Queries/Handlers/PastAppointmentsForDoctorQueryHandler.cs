namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class PastAppointmentsForDoctorQueryHandler : AppRequestHandler<GetPastAppointmentsForDoctorQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<PastAppointmentsForDoctorQueryHandler> logger;

        public PastAppointmentsForDoctorQueryHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IMapper mapper,
            ILogger<PastAppointmentsForDoctorQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetPastAppointmentsForDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctorUserId = await doctorService.GetDoctorUserIdAsync(request.DoctorId, cancellationToken);

            if (doctorUserId == null)
                return NotFound<PagedResult<DoctorAppointmentDTO>>("Doctor not found");

            var authResult = await ValidateOwner(doctorUserId);
            if (authResult != null) return authResult;

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
