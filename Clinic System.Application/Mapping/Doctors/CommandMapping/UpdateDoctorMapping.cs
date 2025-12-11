namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile
    {
        public void UpdateDoctorMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<UpdateDoctorCommand, Doctor>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Doctor, UpdateDoctorDTO>();
        }
    }
}
