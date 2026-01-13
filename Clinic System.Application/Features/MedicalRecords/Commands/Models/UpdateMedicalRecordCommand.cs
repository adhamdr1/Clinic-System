namespace Clinic_System.Application.Features.MedicalRecords.Commands.Models
{
    public class UpdateMedicalRecordCommand : IRequest<Response<UpdateMedicalRecordDTO>>
    {
        // الـ ID مهم جداً عشان نعرف إحنا بنعدل أي سجل
        public int Id { get; set; }

        // الحقول المسموح للدكتور يغيرها فقط
        public string? Diagnosis { get; set; }
        public string? DescriptionOfTheVisit { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
