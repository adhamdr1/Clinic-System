namespace Clinic_System.Application.DTOs.MedicalRecord
{
    public class UpdateMedicalRecordDTO
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDateTime { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
