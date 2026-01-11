namespace Clinic_System.Infrastructure.Services.Email
{
    public class AppointmentEmailNotificationService: IAppointmentNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobService _jobService;
        private readonly IIdentityService _identityService;

        public AppointmentEmailNotificationService(
            IEmailService emailService,
            IBackgroundJobService jobService,
            IIdentityService identityService)
        {
            _emailService = emailService;
            _jobService = jobService;
            _identityService = identityService;
        }

        private async Task EnqueueEmailAsync(
            string userId,
            string subject,
            string body)
        {
            var email = await _identityService.GetUserEmailAsync(userId);

            _jobService.Enqueue(() =>
                _emailService.SendEmailAsync(email, subject, body));
        }

        public async Task SendBookingConfirmationAsync(Appointment appointment)
        {
            var subject = "Your Appointment is Booked - Elite Clinic";

            var body = EmailTemplates.GetBookingConfirmation(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.AppointmentDate);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendCancellationAsync(Appointment appointment)
        {
            var subject = "Your Appointment is Cancelled - Elite Clinic";

            var body = EmailTemplates.GetPatientCancellationEmail(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.AppointmentDate);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendRescheduleAsync(Appointment appointment, DateTime oldDate)
        {
            var subject = "Your Appointment is Rescheduled - Elite Clinic";

            var body = EmailTemplates.GetReschedulingConfirmation(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                oldDate,
                appointment.AppointmentDate);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendNoShowAsync(Appointment appointment)
        {
            var subject = $"Missed Appointment with Dr. {appointment.Doctor.FullName} - Elite Clinic";

            var body = EmailTemplates.GetNoShowNotice(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.AppointmentDate);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendPaymentConfirmationAsync(Appointment appointment)
        {
            var subject = "Your Appointment is Confirmed - Elite Clinic";

            var body = EmailTemplates.GetPaymentAndBookingConfirmation(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.AppointmentDate,
                appointment.Payment.AmountPaid,
                appointment.Payment.PaymentMethod.ToString(),
                appointment.Payment.Id);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendMedicalReportAsync(Appointment appointment)
        {
            var subject = $"Medical Report & Prescription | Dr. {appointment.Doctor.FullName} - Elite Clinic";

            var body = EmailTemplates.GetMedicalReportEmail(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.MedicalRecord.Diagnosis,
                appointment.MedicalRecord.DescriptionOfTheVisit,
                appointment.MedicalRecord.Prescriptions.ToList(),
                appointment.MedicalRecord.AdditionalNotes);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

        public async Task SendAutoCancellationAsync(Appointment appointment)
        {
            var subject = "Your Appointment Reservation has Expired";

            var body = EmailTemplates.GetAutoCancellationEmail(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Doctor.Specialization,
                appointment.AppointmentDate);

            await EnqueueEmailAsync(
                appointment.Patient.ApplicationUserId,
                subject,
                body);
        }

    }
}
