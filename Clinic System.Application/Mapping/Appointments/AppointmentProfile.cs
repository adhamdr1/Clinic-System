namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            GetAvailableSlotMapping();
            BookAppointmentMapping();
        }
    }
}
