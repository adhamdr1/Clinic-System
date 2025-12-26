namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class DoctorAppointmentsQueryHandler : ResponseHandler, IRequestHandler<GetDoctorAppointmentsQuery, Response<PagedResult<DoctorAppointmentDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorAppointmentsQueryHandler> logger;

        public DoctorAppointmentsQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ILogger<DoctorAppointmentsQueryHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<DoctorAppointmentDTO>>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetDoctorAppointments for DoctorId={DoctorId}", request.doctorId);

            if (request.pageNumber < 1)
            {
                logger.LogWarning("Invalid PageNumber={PageNumber} requested", request.pageNumber);
                return BadRequest<PagedResult<DoctorAppointmentDTO>>("Page number must be greater than 0");
            }

            if (request.pageSize < 1 || request.pageSize > 100)
            {
                logger.LogWarning("Invalid PageSize={PageSize} requested", request.pageSize);
                return BadRequest<PagedResult<DoctorAppointmentDTO>>("Page size must be between 1 and 100");
            }

            if(request.doctorId < 1)
            {
                logger.LogWarning("Invalid DoctorId={DoctorId} requested", request.doctorId);
                return BadRequest<PagedResult<DoctorAppointmentDTO>>("DoctorId number must be greater than 0");
            }

            var doctorwithAppointment = await appointmentService.GetDoctorAppointmentsAsync(request);
           
            var doctorwithAppointmentmapper = mapper.Map<List<DoctorAppointmentDTO>>(doctorwithAppointment.Items);

            var pagedResult = new PagedResult<DoctorAppointmentDTO>(doctorwithAppointmentmapper, doctorwithAppointment.TotalCount,
                doctorwithAppointment.CurrentPage, doctorwithAppointment.PageSize);

            logger.LogInformation("Successfully retrieved {Count} appointments for PageNumber={PageNumber}, PageSize={PageSize}", doctorwithAppointment.Items.Count(), request.pageNumber, request.pageSize);

            return Success(pagedResult);
        }
    }
}
