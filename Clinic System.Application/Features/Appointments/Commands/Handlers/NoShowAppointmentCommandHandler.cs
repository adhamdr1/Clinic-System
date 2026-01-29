namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class NoShowAppointmentCommandHandler : AppRequestHandler<NoShowAppointmentCommand, CaneclledAndNoShowAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<NoShowAppointmentCommandHandler> logger;
        public NoShowAppointmentCommandHandler(
             ICurrentUserService currentUserService,
            IAppointmentService appointmentService, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<NoShowAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<CaneclledAndNoShowAppointmentDTO>> Handle(NoShowAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<CaneclledAndNoShowAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidateDoctorAccess(appointment.DoctorId);
            if (authResult != null)
                return authResult;

            request.DoctorId = appointment.DoctorId;

            Appointment NoShowAppointment = null;
            try
            {
                NoShowAppointment = await appointmentService.NoShowAppointmentAsync(request, cancellationToken);

                logger.LogInformation("Starting No-Show process for AppointmentId: {AppointmentId}, PatientId: {PatientId}", request.AppointmentId, NoShowAppointment.PatientId);


                var CaneclledAndNoShowAppointmentDTO = mapper.Map<CaneclledAndNoShowAppointmentDTO>(NoShowAppointment);

                logger.LogInformation("Appointment NoShowed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", NoShowAppointment.PatientId, NoShowAppointment.DoctorId);

                return Success(CaneclledAndNoShowAppointmentDTO, "Appointment NoShowed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while NoShow appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", NoShowAppointment?.PatientId, NoShowAppointment?.DoctorId);
                return BadRequest<CaneclledAndNoShowAppointmentDTO>("Error occurred while processing No Showing: " + ex.Message);
            }
        }
    }
}