namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class AppointmentAutoCancelledEventConsumer : IConsumer<AppointmentAutoCancelledEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentAutoCancelledEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentAutoCancelledEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendAutoCancellationAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.AppointmentDate);
        }
    }
}
