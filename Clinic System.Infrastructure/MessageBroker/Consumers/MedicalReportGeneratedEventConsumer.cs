namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class MedicalReportGeneratedEventConsumer : IConsumer<MedicalReportGeneratedEvent>
    {
        private readonly IAppointmentNotificationService _notificationService;

        public MedicalReportGeneratedEventConsumer(IAppointmentNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<MedicalReportGeneratedEvent> context)
        {
            var data = context.Message;

            await _notificationService.SendMedicalReportAsync(
                data.PatientUserId,
                data.PatientName,
                data.DoctorName,
                data.DoctorSpecialization,
                data.Diagnosis,
                data.Description,
                data.Medicines,
                data.AdditionalNotes);
        }
    }
}
