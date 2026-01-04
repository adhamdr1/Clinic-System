namespace Clinic_System.Application.DTOs.Appointments
{
    public class ConfirmAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDateTime { get; set; }
        public string Status { get; set; }
        public string AppointmentDuration { get; set; }

        public PaymentDTO Payment { get; set; } = new();
    }
}
