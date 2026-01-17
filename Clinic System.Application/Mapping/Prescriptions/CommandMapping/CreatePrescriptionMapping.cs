namespace Clinic_System.Application.Mapping.Prescriptions
{
    public partial class PrescriptionProfile
    {
        public void CreatePrescriptionMapping()
        {
            CreateMap<CreatePrescriptionCommand, Prescription>()
                // الـ Id بتاع الـ Prescription بيتكريت تلقائي، مش محتاجين نماب الـ Id من الـ Command
                .ForMember(dest => dest.MedicalRecordId, opt => opt.MapFrom(src => src.MedicalRecordId))
                .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.MedicationName))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.SpecialInstructions, opt => opt.MapFrom(src => src.SpecialInstructions));
         
        }
    }
}
