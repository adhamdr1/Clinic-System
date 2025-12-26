namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void GetDoctorAppointmentsMapping()
        {
            CreateMap<Appointment, DoctorAppointmentDTO>()
                 .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd HH:mm")))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
