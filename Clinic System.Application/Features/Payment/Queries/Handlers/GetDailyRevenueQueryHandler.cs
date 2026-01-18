namespace Clinic_System.Application.Features.Payment.Queries.Handlers
{
    public class GetDailyRevenueQueryHandler : ResponseHandler, IRequestHandler<GetDailyRevenueQuery, Response<DailyRevenueDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetDailyRevenueQueryHandler> _logger;

        public GetDailyRevenueQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDailyRevenueQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<DailyRevenueDTO>> Handle(GetDailyRevenueQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generating financial report for date: {Date}", request.Date ?? DateTime.Today);

            try
            {
                var targetDate = request.Date ?? DateTime.Today;

                var (total, cash, insta, card, count) = await _unitOfWork.PaymentsRepository.GetDailyTotalsAsync(targetDate);

                var response = new DailyRevenueDTO
                {
                    TotalRevenue = total,
                    CashTotal = cash,
                    InstaPayTotal = insta,
                    CardTotal = card,
                    TotalTransactions = count,
                    ReportDate = targetDate.ToString("yyyy-MM-dd")
                };

                _logger.LogInformation("Daily revenue report generated successfully for date: {Date}", response.ReportDate);
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating daily revenue report.");
                return BadRequest<DailyRevenueDTO>("An error occurred while calculating the revenue.");
            }
        }
    }
}
