namespace Clinic_System.Application.Features.Payment.Queries.Models
{
    public class GetPaymentsListQuery : IRequest<Response<PagedResult<PaymentDetailsDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // الفلاتر (كلها Optional)
        public int? DoctorId { get; set; }       // هاتلي كشوفات الدكتور فلان
        public int? PatientId { get; set; }      // هاتلي مدفوعات المريض ده
        public DateTime? FromDate { get; set; }  // من يوم كذا
        public DateTime? ToDate { get; set; }    // ليوم كذا
        public PaymentMethod? Method { get; set; } // هاتلي الكاش بس
        public PaymentStatus? Status { get; set; } // هاتلي الـ Paid بس
    }
}
