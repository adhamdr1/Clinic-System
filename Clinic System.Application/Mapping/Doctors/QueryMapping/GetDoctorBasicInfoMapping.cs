namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile
    {
        public void GetDoctorBasicInfoMapping()
        {
            CreateMap<Doctor, GetDoctorBasicInfoDTO>()

                .ForMember(dest => dest.Id
                , option => option.MapFrom(src => src.Id))

                .ForMember(dest => dest.FullName
                , option => option.MapFrom(src => src.FullName))

                .ForMember(dest => dest.Specialization
                , option => option.MapFrom(src => src.Specialization));
        }
    }
}