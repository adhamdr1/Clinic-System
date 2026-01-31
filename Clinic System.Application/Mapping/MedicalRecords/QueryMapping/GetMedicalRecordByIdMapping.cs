namespace Clinic_System.Application.Mapping.MedicalRecords
{
    public partial class MedicalRecordProfile
    {
        public void GetMedicalRecordByIdMapping()
        {
            CreateMap<MedicalRecord, MedicalRecordDTO>()
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.AppointmentDateTime,
                           opt => opt.MapFrom(src => src.Appointment.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.FullName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Appointment.Doctor.FullName))
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
