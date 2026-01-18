namespace Clinic_System.Application.Features.Payment.Queries.Handlers
{
    public class PaymentsListQueryHandler : ResponseHandler , IRequestHandler<GetPaymentsListQuery, Response<PagedResult<PaymentDetailsDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<PaymentsListQueryHandler> logger;

        public PaymentsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentsListQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<PaymentDetailsDTO>>> Handle(GetPaymentsListQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPaymentsListQuery");

            var (items, totalCount) = await unitOfWork.PaymentsRepository.GetFilteredPaymentsAsync(
                 request.DoctorId,
                 request.PatientId,
                 request.FromDate,
                 request.ToDate,
                 request.Status,
                 request.Method,
                 request.PageNumber,
                 request.PageSize);


            var dtos = mapper.Map<List<PaymentDetailsDTO>>(items);


            var pagedResultDTO = new PagedResult<PaymentDetailsDTO>(dtos, totalCount, request.PageNumber, request.PageSize);

            logger.LogInformation("Successfully handled GetPaymentsListQuery");
            return Success(pagedResultDTO);
        }
    }
}
