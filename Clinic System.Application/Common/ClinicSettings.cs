namespace Clinic_System.Application.Common
{
    public class ClinicSettings
    {
        public TimeSpan DayStartTime { get; set; }
        public TimeSpan DayEndTime { get; set; }
        public int SlotDurationInMinutes { get; set; }
    }
}
