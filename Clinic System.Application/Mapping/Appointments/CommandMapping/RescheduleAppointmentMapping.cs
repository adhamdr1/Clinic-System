namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void RescheduleAppointmentMapping()
        {
            CreateMap<RescheduleAppointmentCommand, Appointment>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.Date.Add(src.AppointmentTime)));
        }
    }
}
