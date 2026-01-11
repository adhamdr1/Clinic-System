namespace Clinic_System.Application.Service.Interface
{
    public interface IAppointmentNotificationService
    {
        Task SendBookingConfirmationAsync(Appointment appointment);
        Task SendPaymentConfirmationAsync(Appointment appointment);
        Task SendCancellationAsync(Appointment appointment);
        Task SendAutoCancellationAsync(Appointment appointment);
        Task SendRescheduleAsync(Appointment appointment, DateTime oldDate);
        Task SendNoShowAsync(Appointment appointment);
        Task SendMedicalReportAsync(Appointment appointment);
    }
}
