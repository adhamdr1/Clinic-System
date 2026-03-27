namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class AppointmentCancelledEventConsumer : IConsumer<AppointmentCancelledEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentCancelledEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentCancelledEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendCancellationAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.AppointmentDate);
        }
    }
}
