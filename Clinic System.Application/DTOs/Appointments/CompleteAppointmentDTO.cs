namespace Clinic_System.Application.DTOs.Appointments
{
    public class CompleteAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDateTime { get; set; }
        public string Status { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public string? AdditionalNotes { get; set; } = null!;
        public List<PrescriptionDto> Medicines { get; set; } = new();
    }
}
