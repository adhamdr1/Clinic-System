namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile
    {
        public void GetDoctorWithAppointmentsByIdMapping()
        {
            CreateMap<Appointment, GetAppointmentForDoctorDTO>()
                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatientId,
                    opt => opt.MapFrom(src => src.Patient.Id))
                .ForMember(dest => dest.PatientFullName,
                    opt => opt.MapFrom(src => src.Patient.FullName));


            CreateMap<Doctor, GetDoctorDTO>()

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
