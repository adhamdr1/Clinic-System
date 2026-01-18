namespace Clinic_System.Application.Features.Payment.Queries.Handlers
{
    public class GetPaymentDetailsByIdQueryHandler : ResponseHandler , IRequestHandler<GetPaymentDetailsByIdQuery, Response<PaymentDetailsDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<GetPaymentDetailsByIdQueryHandler> logger;

        public GetPaymentDetailsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaymentDetailsByIdQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PaymentDetailsDTO>> Handle(GetPaymentDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await unitOfWork.PaymentsRepository.GetPaymentDetailsByIdAsync(request.Id);
                if (payment == null)
                {
                    logger.LogWarning("Payment with ID {PaymentId} not found.", request.Id);
                    return NotFound<PaymentDetailsDTO>("Payment not found.");
                }
                var paymentDto = mapper.Map<PaymentDetailsDTO>(payment);
                
                return Success(paymentDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving payment details for ID {PaymentId}.", request.Id);
                return BadRequest<PaymentDetailsDTO>("An error occurred while processing your request.");
            }
        }
    }
}
