namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class RescheduleAppointmentCommandHandler : AppRequestHandler<RescheduleAppointmentCommand, AppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RescheduleAppointmentCommandHandler> logger;
        public RescheduleAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RescheduleAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<AppointmentDTO>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<AppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidatePatientAccess(appointment.PatientId);
            if (authResult != null)
                return authResult;

            request.PatientId = appointment.PatientId;



            Appointment newAppointment =null;

            try
            {
                newAppointment = await appointmentService.RescheduleAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<AppointmentDTO>(newAppointment);
               
                logger.LogInformation("Appointment rescheduled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", newAppointment.PatientId, newAppointment.DoctorId);

                return Success(appointmentDto, "Appointment rescheduled successfully.");

            }
            catch (SlotAlreadyBookedException ex) when (ex.Message.Contains("not available"))
            {
                logger.LogWarning("Reschedule failed: {ErrorMessage}", ex.Message);
                return BadRequest<AppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while reschedule appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", newAppointment?.PatientId, newAppointment?.DoctorId);
                return BadRequest<AppointmentDTO>("Error occurred while processing rescheduling: " + ex.Message);
            }
        }
    }
}