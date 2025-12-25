namespace Clinic_System.Application.Service.Interface
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(int appointmentId , CancellationToken cancellationToken = default);
        Task<Payment> FailedPaymentAsync(int appointmentId, CancellationToken cancellationToken = default, string? message = null);
        Task<Payment> ConfirmPaymentAsync(int appointmentId, PaymentMethod? method = null, decimal? amount = null, CancellationToken cancellationToken = default);
    }
}
