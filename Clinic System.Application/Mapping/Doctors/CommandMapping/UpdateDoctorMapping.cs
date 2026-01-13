namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class MedicalRecordProfile
    {
        public void UpdateDoctorMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<UpdateDoctorCommand, Doctor>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
            // الشرط المعدل: لا تنقل القيمة إذا كانت null أو فراغ
                     srcMember != null && (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))
                ));
            CreateMap<Doctor, UpdateDoctorDTO>();
        }
    }
}
