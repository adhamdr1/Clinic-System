namespace Clinic_System.Application.Mapping.MedicalRecords
{
    public partial class MedicalRecordProfile
    {
        public void GetMedicalRecordPatientHistoryMapping()
        {
            CreateMap<MedicalRecord, MedicalRecordPatientHistoryDTO>()
                .ForMember(dest => dest.RecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))

                .ForMember(dest => dest.AppointmentDateTime,
                           opt => opt.MapFrom(src => src.Appointment.AppointmentDate.ToString("dd/MM/yyyy - hh:mm tt")))

                .ForMember(dest => dest.CreatedAt,
                           opt => opt.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy - hh:mm tt")))

                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Appointment.Doctor.FullName))

                .ForMember(dest => dest.DoctorSpecialization,
                           opt => opt.MapFrom(src => src.Appointment.Doctor.Specialization ?? "General"))

                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis ?? "No Diagnosis"))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionOfTheVisit ?? string.Empty))

                .ForMember(dest => dest.Medicines,
                           opt => opt.MapFrom(src => src.Prescriptions != null
                               ? src.Prescriptions
                               : new List<Prescription>()));
        }
    }
}
