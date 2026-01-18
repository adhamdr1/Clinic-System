namespace Clinic_System.Application.Features.Payment.Queries.Models
{
    public class GetPaymentDetailsByIdQuery : IRequest<Response<PaymentDetailsDTO>>
    {
        public int Id { get; set; }
    }
}
