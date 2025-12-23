namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void GetPatientListMapping()
        {
            CreateMap<Patient, GetPatientListDTO>()

                .ForMember(dest => dest.Gender
                , option => option.MapFrom(src => src.Gender.ToString()))

                .ForMember(dest => dest.DateOfBirth
                , option => option.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(dest => dest.CreatedAt
                , option => option.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy-hh:mm")));
        }
    }
}
