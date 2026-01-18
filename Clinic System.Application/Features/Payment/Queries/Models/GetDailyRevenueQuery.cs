namespace Clinic_System.Application.Features.Payment.Queries.Models
{
    public class GetDailyRevenueQuery : IRequest<Response<DailyRevenueDTO>>
    {
        public DateTime? Date { get; set; }
    }
}
