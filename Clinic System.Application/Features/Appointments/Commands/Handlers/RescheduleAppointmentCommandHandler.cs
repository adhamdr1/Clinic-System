namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class RescheduleAppointmentCommandHandler : AppRequestHandler<RescheduleAppointmentCommand, AppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RescheduleAppointmentCommandHandler> logger;
        public RescheduleAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
                ICacheService cacheService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RescheduleAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
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



            Appointment RescheduleAppointment = null;

            try
            {
                RescheduleAppointment = await appointmentService.RescheduleAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<AppointmentDTO>(RescheduleAppointment);
               
                logger.LogInformation("Appointment rescheduled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", RescheduleAppointment.PatientId, RescheduleAppointment.DoctorId);

                await cacheService.RemoveByPrefixAsync(
                    $"UpcomingAppts_Patient_{RescheduleAppointment.PatientId}",
                    $"UpcomingAppts_Doctor_{RescheduleAppointment.DoctorId}",
                    $"DoctorApptsByStatus_{RescheduleAppointment.DoctorId}",
                    "AdminApptsByStatus",
                    "AdminStats"
                );

                return Success(appointmentDto, "Appointment rescheduled successfully.");

            }
            catch (SlotAlreadyBookedException ex) when (ex.Message.Contains("not available"))
            {
                logger.LogWarning("Reschedule failed: {ErrorMessage}", ex.Message);
                return BadRequest<AppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while reschedule appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", RescheduleAppointment?.PatientId, RescheduleAppointment?.DoctorId);
                return BadRequest<AppointmentDTO>("Error occurred while processing rescheduling: " + ex.Message);
            }
        }
    }
}