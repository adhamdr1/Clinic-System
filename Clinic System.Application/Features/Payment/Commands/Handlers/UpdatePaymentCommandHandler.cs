namespace Clinic_System.Application.Features.Payment.Commands.Handlers
{
    public class UpdatePaymentCommandHandler : ResponseHandler , IRequestHandler<UpdatePaymentCommand, Response<PaymentDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePaymentCommandHandler> _logger;
       
        public UpdatePaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdatePaymentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<PaymentDTO>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting update process for payment with ID {PaymentId}", request.PaymentId);
            try
            {
                var payment = await _unitOfWork.PaymentsRepository.GetByIdAsync(request.PaymentId);
                if (payment == null)
                {
                    return NotFound<PaymentDTO>($"Payment with ID {request.PaymentId} not found.");
                }

                payment.UpdatePaymentDetails(request.Amount, request.PaymentMethod, request.Notes);

                _unitOfWork.PaymentsRepository.Update(payment);
                var result = await _unitOfWork.SaveAsync();
                if (result < 1)
                {
                    _logger.LogWarning("No changes were saved when updating payment with ID {PaymentId}", request.PaymentId);
                    return BadRequest<PaymentDTO>("Failed to update the payment.");
                }
                var paymentDto = _mapper.Map<PaymentDTO>(payment);

                _logger.LogInformation("Payment with ID {PaymentId} updated successfully", request.PaymentId);
                return Success(paymentDto, "Payment updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment with ID {PaymentId}", request.PaymentId);
                return BadRequest<PaymentDTO>("An error occurred while updating the payment.");
            }
        }
    }
}
