namespace Clinic_System.Application.Service.Interface
{
    public interface IAppointmentNotificationService
    {
        Task SendBookingConfirmationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate);
        Task SendPaymentConfirmationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate, decimal amountPaid, string paymentMethod, int transactionId);
        Task SendCancellationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate);
        Task SendAutoCancellationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate);
        Task SendRescheduleAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime oldDate, DateTime newDate);
        Task SendNoShowAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate);
        Task SendMedicalReportAsync(
            string patientUserId,
            string patientName,
            string doctorName,
            string specialty,
            string diagnosis,
            string description,
            List<MedicationInfo> medicines,
            string? additionalNotes);
    }
}
