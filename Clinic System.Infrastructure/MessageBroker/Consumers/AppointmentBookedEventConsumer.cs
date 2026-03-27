namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    // الكلاس ده بـ "يستهلك" أو "بيسمع" الحدث بتاع حجز الموعد
    public class AppointmentBookedEventConsumer : IConsumer<AppointmentBookedEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentBookedEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var data = context.Message;

            // بنباصي الداتا الخام لخدمة الإيميل
            await _notificationService.SendBookingConfirmationAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.AppointmentDate);
        }
    }
}