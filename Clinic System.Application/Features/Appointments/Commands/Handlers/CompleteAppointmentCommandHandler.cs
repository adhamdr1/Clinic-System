namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CompleteAppointmentCommandHandler : AppRequestHandler<CompleteAppointmentCommand, CompleteAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationsService notificationsService;
        private readonly ILogger<CompleteAppointmentCommandHandler> logger;
        public CompleteAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            INotificationsService notificationService,
            ICacheService cacheService,
            IUnitOfWork unitOfWork,
            ILogger<CompleteAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.notificationsService = notificationService;
            this.logger = logger;
        }

        public override async Task<Response<CompleteAppointmentDTO>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<CompleteAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidateDoctorAccess(appointment.DoctorId);
            if (authResult != null)
                return authResult;

            request.DoctorId = appointment.DoctorId;

            Appointment CompleteAppointment = null;
            try
            {
                CompleteAppointment = await appointmentService.CompleteAppointmentAsync(request, cancellationToken);

                logger.LogInformation("Appointment completed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment.PatientId, CompleteAppointment.DoctorId);

                var CompleteAppointmentDTO = mapper.Map<CompleteAppointmentDTO>(CompleteAppointment);

                logger.LogInformation("Appointment Completeed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment.PatientId, CompleteAppointment.DoctorId);


                await cacheService.RemoveByPrefixAsync(
                    $"UpcomingAppts_Patient_{CompleteAppointment.PatientId}",
                    $"PastAppts_Patient_{CompleteAppointment.PatientId}",      // مسحنا الماضي كمان لأنه ممكن يظهر فيه كـ ملغي
                    $"UpcomingAppts_Doctor_{CompleteAppointment.DoctorId}",
                    $"PastAppts_Doctor_{CompleteAppointment.DoctorId}",        // مسحنا الماضي
                    $"DoctorApptsByStatus_{CompleteAppointment.DoctorId}",
                    "AdminApptsByStatus",
                    "AdminStats"
                );

                var notificationDto = new NotificationDTO
                {
                    Title = "Prescription & Record Ready",
                    Message = $"Doctor has completed the appointment for '{CompleteAppointmentDTO.PatientName}'. Medical record and prescription are ready.",
                    NotificationType = "AppointmentCompleted",
                    RelatedEntityId = CompleteAppointment.Id
                };

                await notificationsService.SendToGroupAsync("Admins", notificationDto);

                return Success(CompleteAppointmentDTO, "Appointment Completeed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Complete appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment?.PatientId, CompleteAppointment?.DoctorId);
                return BadRequest<CompleteAppointmentDTO>("Error occurred while processing Completing: " + ex.Message);
            }
        }
    }
}