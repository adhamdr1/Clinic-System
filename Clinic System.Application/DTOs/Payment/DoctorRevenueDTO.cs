namespace Clinic_System.Application.DTOs.Payment
{
    public class DoctorRevenueDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int CompletedAppointmentsCount { get; set; }
        public string PeriodFrom { get; set; } = string.Empty;
        public string PeriodTo { get; set; } = string.Empty;
    }
}
