namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void CompleteGoogleRegistrationMapping()
        {
            CreateMap<CompleteGoogleRegistrationCommand, Patient>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());
        }
    }
}
