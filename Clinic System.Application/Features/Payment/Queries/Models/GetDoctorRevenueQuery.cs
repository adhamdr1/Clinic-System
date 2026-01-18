namespace Clinic_System.Application.Features.Payment.Queries.Models
{
    public class GetDoctorRevenueQuery : IRequest<Response<DoctorRevenueDTO>>
    {
        public int DoctorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
