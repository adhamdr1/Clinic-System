namespace Clinic_System.Application.Mapping.MedicalRecords
{
    public partial class MedicalRecordProfile
    {
        public void UpdateMedicalRecordMapping()
        {
            // 1. من الـ Entity إلى الـ UpdateMedicalRecordDTO (الـ Response اللي بيرجع بعد الحفظ)
            CreateMap<MedicalRecord, UpdateMedicalRecordDTO>()
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.FullName))
                .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.Appointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm")))
                // هنا بنحول الـ UpdatedAt لـ string عشان الـ UI
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionOfTheVisit));
        }
    }
}
