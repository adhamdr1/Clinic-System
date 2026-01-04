namespace Clinic_System.Application.DTOs.Appointments
{
    public class AppointmentStatsDto
    {
        public int TotalAppointments { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Rescheduled { get; set; }
        public int Confirmed { get; set; }
        public int Cancelled { get; set; }
        public int NoShow { get; set; }
    }
}
