namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class CompleteAppointmentCommand
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public List<PrescriptionDto> Medicines { get; set; } = new();
    }
}
