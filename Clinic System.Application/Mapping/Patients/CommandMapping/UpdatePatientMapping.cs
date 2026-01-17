namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void UpdatePatientMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<UpdatePatientCommand, Patient>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                     // الشرط المعدل: لا تنقل القيمة إذا كانت null أو فراغ
                     srcMember != null && (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))
                ));
            CreateMap<Patient, UpdatePatientDTO>();
        }
    }
}
