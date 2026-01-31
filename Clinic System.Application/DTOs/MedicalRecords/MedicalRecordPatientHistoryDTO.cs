namespace Clinic_System.Application.DTOs.MedicalRecord
{
    public class MedicalRecordPatientHistoryDTO
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }

        // معلومات الزيارة
        public string AppointmentDateTime { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;

        // معلومات الدكتور (عشان نعرف التخصص)
        public string DoctorName { get; set; } = null!;
        public string DoctorSpecialization { get; set; } = null!; // <--- إضافة مهمة

        // التفاصيل الطبية
        public string Diagnosis { get; set; } = null!;
        public string? Description { get; set; } // <--- عشان نفهم الأعراض كانت إيه

        public List<PrescriptionDto> Medicines { get; set; }
    }
}
