namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void BookAppointmentMapping()
        {
            CreateMap<BookAppointmentCommand, Appointment>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.Date.Add(src.AppointmentTime)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending));

        }
    }
}
