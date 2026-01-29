namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CancelAppointmentCommandHandler : AppRequestHandler<CancelAppointmentCommand, CaneclledAndNoShowAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CancelAppointmentCommandHandler> logger;
        public CancelAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
           ILogger<CancelAppointmentCommandHandler> logger,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<CaneclledAndNoShowAppointmentDTO>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<CaneclledAndNoShowAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidatePatientAccess(appointment.PatientId);
            if (authResult != null)
                return authResult;

            request.PatientId = appointment.PatientId;

            Appointment CancelAppointment = null;
            try
            {
                CancelAppointment = await appointmentService.CancelAppointmentAsync(request, cancellationToken);

                var CaneclledAndNoShowAppointmentDTO = mapper.Map<CaneclledAndNoShowAppointmentDTO>(CancelAppointment);

                logger.LogInformation("Appointment Cancelled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment.PatientId, CancelAppointment.DoctorId);

                return Success(CaneclledAndNoShowAppointmentDTO, "Appointment Cancelled successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Cancel appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment?.PatientId, CancelAppointment?.DoctorId);
                return BadRequest<CaneclledAndNoShowAppointmentDTO>("Error occurred while processing Cancelling: " + ex.Message);
            }
        }
    }
}