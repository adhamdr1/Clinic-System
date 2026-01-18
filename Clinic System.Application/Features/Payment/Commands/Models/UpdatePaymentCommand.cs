namespace Clinic_System.Application.Features.Payment.Commands.Models
{
    public class UpdatePaymentCommand : IRequest<Response<PaymentDTO>>
    {
        public int PaymentId { get; set; }

        // خليناها Nullable عشان لو عايز يعدل حاجة ويسيب التانية
        public decimal? Amount { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }
}
