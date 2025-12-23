namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void GetPatientWithAppointmentsByIdMapping()
        {
            CreateMap<Appointment, GetAppointmentForPatientDTO>()
                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DoctorId,
                    opt => opt.MapFrom(src => src.Doctor.Id))
                .ForMember(dest => dest.DoctorFullName,
                    opt => opt.MapFrom(src => src.Doctor.FullName));


            CreateMap<Patient, GetPatientWhitAppointmentDTO>()

                .ForMember(dest => dest.Gender
                , option => option.MapFrom(src => src.Gender.ToString()))

                .ForMember(dest => dest.DateOfBirth
                , option => option.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(dest => dest.CreatedAt
                , option => option.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy-HH:mm")))

                .ForMember(dest => dest.Appointments
                , option => option.MapFrom(src => src.Appointments));

        }
    }
}
