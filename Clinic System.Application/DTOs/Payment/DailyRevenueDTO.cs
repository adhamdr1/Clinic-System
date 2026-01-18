namespace Clinic_System.Application.DTOs.Payment
{
    public class DailyRevenueDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal CashTotal { get; set; }
        public decimal InstaPayTotal { get; set; }
        public decimal CardTotal { get; set; }
        public int TotalTransactions { get; set; }
        public string ReportDate { get; set; }
    }
}
