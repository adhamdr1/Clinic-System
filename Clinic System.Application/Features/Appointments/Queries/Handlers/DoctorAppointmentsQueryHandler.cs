namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class DoctorAppointmentsQueryHandler : AppRequestHandler<GetDoctorAppointmentsQuery, PagedResult<DoctorAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService; 
        private readonly IMapper mapper;
        private readonly ILogger<DoctorAppointmentsQueryHandler> logger;

        public DoctorAppointmentsQueryHandler(
            ICurrentUserService currentUserService, 
            IAppointmentService appointmentService,
            IDoctorService doctorService, 
            IMapper mapper,
            ILogger<DoctorAppointmentsQueryHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var doctorUserId = await doctorService.GetDoctorUserIdAsync(request.DoctorId, cancellationToken);

            if (doctorUserId == null)
                return NotFound<PagedResult<DoctorAppointmentDTO>>("Doctor not found");

            var authResult = await ValidateOwner(doctorUserId);
            if (authResult != null) return authResult;

            logger.LogInformation("Starting GetDoctorAppointments for DoctorId={DoctorId}", request.DoctorId);

            var doctorwithAppointment = await appointmentService.GetDoctorAppointmentsAsync(request);
           
            var doctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(doctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(doctorwithAppointmentmapper, doctorwithAppointment.TotalCount,
                doctorwithAppointment.CurrentPage, doctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", doctorwithAppointment.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
