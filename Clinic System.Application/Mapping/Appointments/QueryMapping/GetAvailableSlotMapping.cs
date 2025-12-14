namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void GetAvailableSlotMapping()
        {
            CreateMap<TimeSpan, AvailableSlotDTO>()
            .ForMember(dest => dest.SlotTime, opt => opt.MapFrom(src => src));
        }
    }
}
