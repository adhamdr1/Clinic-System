namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void GetPatientAppointmentsMapping()
        {
            CreateMap<Appointment, PatientAppointmentDTO>()
                 .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd HH:mm")))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                 .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                 .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Doctor.Specialization));
        }
    }
}
