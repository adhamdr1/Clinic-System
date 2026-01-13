namespace Clinic_System.Application.DTOs.MedicalRecord
{
    public class MedicalRecordDoctorDTO
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = null!;
        public string AppointmentDateTime { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
    }
}
