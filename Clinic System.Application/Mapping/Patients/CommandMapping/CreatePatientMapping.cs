namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void CreatePatientMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<CreatePatientCommand, Patient>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore()) // اليوزر ID هنجيبه يدوي
                .ForMember(dest => dest.Appointments, opt => opt.Ignore()); // نتجاهل الليستات

            CreateMap<Patient, CreatePatientDTO>()
                // ⭐️ تحسين: تحويل الـ Enum إلى String ليكون واضحاً في الـ JSON
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))

                // ⭐️ تحسين: تحويل التاريخ إلى String بتنسيق موحد (يفترض DTO.DateOfBirth هو String)
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(dest => dest.CreatedAt, option => option.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy-hh:mm")));
        }
    }
}
