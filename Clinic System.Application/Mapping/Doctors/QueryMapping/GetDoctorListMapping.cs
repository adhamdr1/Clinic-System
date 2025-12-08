namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile
    {
        public void GetDoctorListMapping()
        {
            CreateMap<Doctor, GetDoctorListDTO>()

                .ForMember(dest => dest.Gender
                , option => option.MapFrom(src => src.Gender.ToString()))

                .ForMember(dest => dest.DateOfBirth
                , option => option.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(dest => dest.CreatedAt
                , option => option.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy")));
        }
    }
}
