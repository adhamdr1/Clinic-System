namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class AppointmentNoShowEventConsumer : IConsumer<AppointmentNoShowEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentNoShowEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentNoShowEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendNoShowAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.AppointmentDate);
        }
    }
}
