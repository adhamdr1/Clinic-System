namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void CancelAppointmentMapping()
        {
            CreateMap<Appointment, CaneclledAndNoShowAppointmentDTO>()
               .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
               .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));
        }
    }
}