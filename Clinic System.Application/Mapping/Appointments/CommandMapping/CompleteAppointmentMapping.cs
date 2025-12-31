namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void CompleteAppointmentMapping()
        {
            // Mapping for Prescription → PrescriptionDto
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.MedicationName))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.SpecialInstructions, opt => opt.MapFrom(src => src.SpecialInstructions));

            // Mapping for Appointment → CompleteAppointmentDTO
            CreateMap<Appointment, CompleteAppointmentDTO>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AppointmentDateTime,
                           opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.MedicalRecord.Diagnosis ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.MedicalRecord.DescriptionOfTheVisit ?? string.Empty))
                .ForMember(dest => dest.AdditionalNotes, opt => opt.MapFrom(src => src.MedicalRecord.AdditionalNotes ?? string.Empty))
                .ForMember(dest => dest.Medicines,
                           opt => opt.MapFrom(src => src.MedicalRecord != null
                               ? src.MedicalRecord.Prescriptions
                               : new List<Prescription>()));
        }
    }
}