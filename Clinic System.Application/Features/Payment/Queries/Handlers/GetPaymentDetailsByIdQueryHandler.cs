namespace Clinic_System.Application.Features.Payment.Queries.Handlers
{
    public class GetPaymentDetailsByIdQueryHandler : AppRequestHandler<GetPaymentDetailsByIdQuery, PaymentDetailsDTO>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<GetPaymentDetailsByIdQueryHandler> logger;

        public GetPaymentDetailsByIdQueryHandler(
            ICurrentUserService currentUserService, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetPaymentDetailsByIdQueryHandler> logger) : base(currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PaymentDetailsDTO>> Handle(GetPaymentDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await unitOfWork.PaymentsRepository.GetPaymentDetailsByIdAsync(request.Id);
                if (payment == null)
                {
                    logger.LogWarning("Payment with ID {PaymentId} not found.", request.Id);
                    return NotFound<PaymentDetailsDTO>("Payment not found.");
                }

                var doctorId = payment.Appointment?.DoctorId;
                var patientId = payment.Appointment?.PatientId;

                var roles = await _currentUserService.GetCurrentUserRolesAsync();
                if (!roles.Contains("Admin"))
                {
                    if (CurrentDoctorId.HasValue)
                    {
                        if (doctorId != CurrentDoctorId.Value)
                        {
                            logger.LogWarning("Unauthorized access attempt by Doctor ID {DoctorId} for Payment ID {PaymentId}.", CurrentDoctorId.Value, request.Id);
                            return Unauthorized<PaymentDetailsDTO>("You are not authorized to access this payment.");
                        }
                    }
                    else if (CurrentPatientId.HasValue)
                    {
                        if (patientId != CurrentPatientId.Value)
                        {
                            logger.LogWarning("Unauthorized access attempt by Patient ID {PatientId} for Payment ID {PaymentId}.", CurrentPatientId.Value, request.Id);
                            return Unauthorized<PaymentDetailsDTO>("You are not authorized to access this payment.");
                        }
                    }
                    else
                    {
                        logger.LogWarning("Unauthorized access attempt for Payment ID {PaymentId} with no user context.", request.Id);
                        return Unauthorized<PaymentDetailsDTO>("You are not authorized to access this payment.");
                    }
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
