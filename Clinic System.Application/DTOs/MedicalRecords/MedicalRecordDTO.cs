namespace Clinic_System.Application.DTOs.MedicalRecord
{
    public class MedicalRecordDTO
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDateTime { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public string? AdditionalNotes { get; set; }
        public List<PrescriptionDto> Medicines { get; set; }
    }
}
