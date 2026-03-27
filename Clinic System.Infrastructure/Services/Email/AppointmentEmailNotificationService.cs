namespace Clinic_System.Infrastructure.Services.Email
{
    public class AppointmentEmailNotificationService: IAppointmentNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public AppointmentEmailNotificationService(
            IEmailService emailService,
            IIdentityService identityService)
        {
            _emailService = emailService;
            _identityService = identityService;
        }

        private async Task SendEmailDirectlyAsync(
            string userId,
            string subject,
            string body)
        {
            var email = await _identityService.GetUserEmailAsync(userId);

            await _emailService.SendEmailAsync(email, subject, body);
        }

        public async Task SendBookingConfirmationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate)
        {
            var subject = "Your Appointment is Booked - Elite Clinic";

            var body = EmailTemplates.GetBookingConfirmation(
                patientName,
                doctorName,
                specialization,
                appointmentDate);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendCancellationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate)
        {
            var subject = "Your Appointment is Cancelled - Elite Clinic";

            var body = EmailTemplates.GetPatientCancellationEmail(
                patientName,
                doctorName,
                specialization,
                appointmentDate);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendRescheduleAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime oldDate, DateTime newDate)
        {
            var subject = "Your Appointment is Rescheduled - Elite Clinic";

            var body = EmailTemplates.GetReschedulingConfirmation(
                patientName,
                doctorName,
                specialization,
                oldDate,
                newDate);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendNoShowAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate)
        {
            var subject = $"Missed Appointment with Dr. {doctorName} - Elite Clinic";

            var body = EmailTemplates.GetNoShowNotice(
                patientName,
                doctorName,
                specialization,
                appointmentDate);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendPaymentConfirmationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate, decimal amountPaid, string paymentMethod, int transactionId)
        {
            var subject = "Your Appointment is Confirmed - Elite Clinic";

            var body = EmailTemplates.GetPaymentAndBookingConfirmation(
                patientName,
                doctorName,
                specialization,
                appointmentDate,
                amountPaid,
                paymentMethod,
                transactionId);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendMedicalReportAsync(
            string patientUserId,
            string patientName,
            string doctorName,
            string specialty,
            string diagnosis,
            string description,
            List<MedicationInfo> medicines,
            string? additionalNotes)
        {
            var subject = $"Medical Report & Prescription | Dr. {doctorName} - Elite Clinic";

            var body = EmailTemplates.GetMedicalReportEmail(
                patientName,
                doctorName,
                specialty,
                diagnosis,
                description,
                medicines,
                additionalNotes);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }

        public async Task SendAutoCancellationAsync(string patientUserId, string patientName, string doctorName, string specialization, DateTime appointmentDate)
        {
            var subject = "Your Appointment Reservation has Expired";

            var body = EmailTemplates.GetAutoCancellationEmail(
                patientName,
                doctorName,
                specialization,
                appointmentDate);

            await SendEmailDirectlyAsync(patientUserId, subject, body);
        }
    }
}
