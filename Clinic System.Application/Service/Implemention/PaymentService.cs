namespace Clinic_System.Application.Service.Implemention
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Payment> ConfirmPaymentAsync(int appointmentId, PaymentMethod method,
           string? notes = null, decimal? amount = null, CancellationToken cancellationToken = default)
        {
            var payment = await unitOfWork.PaymentsRepository.GetPaymentByAppointmentIdAsync(appointmentId);

            if (payment == null || payment.PaymentStatus == PaymentStatus.Paid)
            {
                throw new NotFoundException($"No pending payment found for appointment ID {appointmentId}.");
            }

            payment.MarkAsPaid(method,notes, amount);

            unitOfWork.PaymentsRepository.Update(payment, cancellationToken);

            return payment;
        }

        public async Task<Payment> CreatePaymentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {

            var payment = new Payment
            {
                AppointmentId = appointmentId,
                AmountPaid = 300m,
                PaymentStatus = PaymentStatus.Pending,
                PaymentDate = null
            };
            await unitOfWork.PaymentsRepository.AddAsync(payment, cancellationToken);
            return payment;
        }

        public async Task<Payment> FailedPaymentAsync(int appointmentId, CancellationToken cancellationToken = default 
            ,string? message = null)
        {
            var payment = await unitOfWork.PaymentsRepository.GetPaymentByAppointmentIdAsync(appointmentId);

            if (payment == null || payment.PaymentStatus == PaymentStatus.Paid)
            {
                throw new NotFoundException($"No pending payment found for appointment ID {appointmentId}.");
            }

            payment.MarkAsFailed(message);

            unitOfWork.PaymentsRepository.Update(payment, cancellationToken);

            return payment;
        }
    }
}
