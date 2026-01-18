namespace Clinic_System.Application.Features.Payments.Queries.Handlers
{
    public class GetDoctorRevenueQueryHandler : ResponseHandler, IRequestHandler<GetDoctorRevenueQuery, Response<DoctorRevenueDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetDoctorRevenueQueryHandler> _logger;

        public GetDoctorRevenueQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDoctorRevenueQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<DoctorRevenueDTO>> Handle(GetDoctorRevenueQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calculating revenue for Doctor ID: {DoctorId}", request.DoctorId);

            // 1. تحديد التواريخ الافتراضية (اليوم)
            // إذا كان FromDate فارغاً، نأخذ بداية اليوم (الساعة 00:00:00)
            var from = request.FromDate ?? DateTime.Today;

            // إذا كان ToDate فارغاً، نأخذ نهاية اليوم (الساعة 23:59:59)
            var to = request.ToDate ?? DateTime.Today.AddDays(1).AddTicks(-1);

            // 2. التحقق من وجود الدكتور
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