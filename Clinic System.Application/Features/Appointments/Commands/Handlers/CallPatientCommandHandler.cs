namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CallPatientCommandHandler : AppRequestHandler<CallPatientCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationsService notificationsService;
        private readonly ILogger<CallPatientCommandHandler> logger;

        public CallPatientCommandHandler(
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            INotificationsService notificationsService,
            ILogger<CallPatientCommandHandler> logger) : base(currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.notificationsService = notificationsService;
            this.logger = logger;
        }

        public override async Task<Response<string>> Handle(CallPatientCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Attempting to call patient for AppointmentId: {AppointmentId}", request.AppointmentId);

            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                return NotFound<string>("Appointment not found.");
            }

            var authResult = await ValidateDoctorAccess(appointment.DoctorId);
            if (authResult != null)
                return authResult;

            string patientName = appointment.Patient?.FullName ?? $"ID: {appointment.PatientId}";
            string doctorName = appointment.Doctor?.FullName ?? $"ID: {appointment.DoctorId}";

            var notificationDto = new NotificationDTO
            {
                Title = "Patient Call",
                Message = $"Patient '{patientName}' please proceed to Doctor '{doctorName}' clinic.",
                NotificationType = "PatientCalled", 
                RelatedEntityId = appointment.Id
            };

            await notificationsService.SendToGroupAsync("WaitingRoomScreens", notificationDto);

            return Success("Patient called successfully to the waiting room screen.", "successfully for call patiant");
        }
    }
}
