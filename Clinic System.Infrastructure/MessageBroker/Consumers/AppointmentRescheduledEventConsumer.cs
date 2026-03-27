namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class AppointmentRescheduledEventConsumer : IConsumer<AppointmentRescheduledEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentRescheduledEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentRescheduledEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendRescheduleAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.OldDate,
                data.NewDate); // بياخد الميعاد القديم والجديد
        }
    }
}
