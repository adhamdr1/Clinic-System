namespace Clinic_System.Application.Features.Payments.Queries.Handlers
{
    public class GetDoctorRevenueQueryHandler : AppRequestHandler<GetDoctorRevenueQuery, DoctorRevenueDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetDoctorRevenueQueryHandler> _logger;

        // 2. تمرير ICurrentUserService للـ Base Class
        public GetDoctorRevenueQueryHandler(
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ILogger<GetDoctorRevenueQueryHandler> logger) : base(currentUserService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task<Response<DoctorRevenueDTO>> Handle(GetDoctorRevenueQuery request, CancellationToken cancellationToken)
        {
            var (authorizedId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

            if (errorResponse != null) return errorResponse;

            request.DoctorId = authorizedId;

            _logger.LogInformation("Calculating revenue for Doctor ID: {DoctorId}", request.DoctorId);

            
            var from = request.FromDate ?? DateTime.Today;

            // إذا كان ToDate فارغاً، نأخذ نهاية اليوم (الساعة 23:59:59)
            var to = request.ToDate ?? DateTime.Today.AddDays(1).AddTicks(-1);

            var doctor = await _unitOfWork.DoctorsRepository.GetByIdAsync(request.DoctorId);

            if (doctor == null)
                return NotFound<DoctorRevenueDTO>("Doctor not found.");

            // 3. استدعاء الريبوزيتوري بالتواريخ المحددة (سواء المرسلة أو الافتراضية)
            var (total, count) = await _unitOfWork.PaymentsRepository
                .GetDoctorRevenueStatsAsync(request.DoctorId, from, to, cancellationToken);

            // 4. بناء النتيجة
            var result = new DoctorRevenueDTO
            {
                DoctorId = request.DoctorId,
                DoctorName = doctor.FullName,
                TotalRevenue = total,
                CompletedAppointmentsCount = count,
                PeriodFrom = from.ToString("yyyy-MM-dd HH:mm"),
                PeriodTo = to.ToString("yyyy-MM-dd HH:mm")
            };

            return Success(result);
        }
    }
}