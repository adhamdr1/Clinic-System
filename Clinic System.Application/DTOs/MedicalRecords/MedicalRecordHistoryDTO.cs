namespace Clinic_System.Application.DTOs.MedicalRecord
{
    public class MedicalRecordHistoryDTO
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public DateTime CreatedAt { get; set; } // عشان ترتب بيهم في الـ UI
        public string AppointmentDate { get; set; } = null!;
    }
}
