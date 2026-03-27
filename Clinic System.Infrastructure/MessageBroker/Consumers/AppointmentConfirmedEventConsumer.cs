namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class AppointmentConfirmedEventConsumer : IConsumer<AppointmentConfirmedEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public AppointmentConfirmedEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<AppointmentConfirmedEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendPaymentConfirmationAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.AppointmentDate,
                data.AmountPaid,
                data.PaymentMethod,
                data.TransactionId);
        }
    }
}
