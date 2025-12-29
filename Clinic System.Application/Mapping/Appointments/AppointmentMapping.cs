namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void AppointmentMapping()
        {
            CreateMap<Appointment, AppointmentDTO>()
                   .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                   .ForMember(dest => dest.AppointmentDuration, opt => opt.MapFrom(src => "15 Minutes"))
                   .ForMember(dest => dest.DoctorName, opt => opt.Ignore())
                   .ForMember(dest => dest.PatientName, opt => opt.Ignore());
        }
    }
}