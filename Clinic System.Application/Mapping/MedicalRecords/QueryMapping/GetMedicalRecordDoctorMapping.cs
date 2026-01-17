namespace Clinic_System.Application.Mapping.MedicalRecords
{
    public partial class MedicalRecordProfile
    {
        public void GetMedicalRecordDoctorMapping()
        {
            CreateMap<MedicalRecord, MedicalRecordDoctorDTO>()
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.AppointmentDateTime,
                           opt => opt.MapFrom(src => src.Appointment.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.FullName))
                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis ?? string.Empty))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy-HH:mm")));
        }
    }
}
