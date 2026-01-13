namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class MedicalRecordProfile
    {
        public void UpdateIdentityDoctorMapping()
        {
            CreateMap<UpdateIdentityDoctorCommand, Doctor>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore())
              .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                     // الشرط المعدل: لا تنقل القيمة إذا كانت null أو فراغ
                     srcMember != null && (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))
                )); ; ;


            CreateMap<Doctor, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());
        }
    }
}
