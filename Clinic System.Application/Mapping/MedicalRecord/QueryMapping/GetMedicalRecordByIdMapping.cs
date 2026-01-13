namespace Clinic_System.Application.Mapping.MedicalRecords
{
    public partial class MedicalRecordProfile
    {
        public void GetMedicalRecordByIdMapping()
        {
            // Mapping for Prescription → PrescriptionDto
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.MedicationName))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.SpecialInstructions, opt => opt.MapFrom(src => src.SpecialInstructions));

            CreateMap<MedicalRecord, MedicalRecordDTO>()
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.AppointmentDateTime,
                           opt => opt.MapFrom(src => src.Appointment.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.FullName))
                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionOfTheVisit ?? string.Empty))
                .ForMember(dest => dest.AdditionalNotes, opt => opt.MapFrom(src => src.AdditionalNotes ?? string.Empty))
                .ForMember(dest => dest.Medicines,
                           opt => opt.MapFrom(src => src.Prescriptions != null
                               ? src.Prescriptions
                               : new List<Prescription>()));
        }
    }
}
