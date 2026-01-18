namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<Payment> Items, int TotalCount)> GetFilteredPaymentsAsync(
        int? doctorId,
        int? patientId,
        DateTime? fromDate,
        DateTime? toDate,
        PaymentStatus? status,
        PaymentMethod? method,
        int pageNumber,
        int pageSize)
        {
            // 1. Start Query with Includes (عشان نجيب الأسماء)
            var query = context.Payments
                .AsNoTracking()
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient) // عشان اسم المريض
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)  // عشان اسم الدكتور
                .AsQueryable();

            // 2. Dynamic Filtering (تطبيق الفلاتر فقط لو مبعوتة)

            if (doctorId.HasValue)
                query = query.Where(p => p.Appointment.DoctorId == doctorId);

            if (patientId.HasValue)
                query = query.Where(p => p.Appointment.PatientId == patientId);

            if (fromDate.HasValue)
                query = query.Where(p => p.PaymentDate >= fromDate);

            if (toDate.HasValue)
                query = query.Where(p => p.PaymentDate <= toDate);

            if (status.HasValue)
                query = query.Where(p => p.PaymentStatus == status);

            if (method.HasValue)
                query = query.Where(p => p.PaymentMethod == method);

            // 3. Ordering (الأحدث فالأقدم)
            query = query.OrderByDescending(p => p.PaymentDate);

            // 4. Get Total Count (قبل الـ Pagination)
            var totalCount = await query.CountAsync();

            // 5. Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Payment> GetPaymentDetailsByIdAsync(int id)
        {
            return await context.Payments
                .AsNoTracking()
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetPaymentByAppointmentIdAsync(int appointmentId)
        {
            return await context.Payments
                .FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }

        public async Task<(decimal total, decimal cash, decimal insta, decimal card, int count)> GetDailyTotalsAsync(DateTime date , CancellationToken cancellationToken = default)
        {
            var stats = await context.Payments
                 .AsNoTracking()
                 .Where(p => p.PaymentDate.HasValue &&
                             p.PaymentDate.Value.Date == date.Date &&
                             p.PaymentStatus == PaymentStatus.Paid)
                 .GroupBy(p => 1) // تجميع وهمي عشان نقدر نستخدم Sum و Count في خطوة واحدة
                 .Select(g => new
                 {
                     Total = g.Sum(p => p.AmountPaid),
                     Cash = g.Where(p => p.PaymentMethod == PaymentMethod.Cash).Sum(p => p.AmountPaid),
                     Card = g.Where(p => p.PaymentMethod == PaymentMethod.CreditCard).Sum(p => p.AmountPaid),
                     Insta = g.Where(p => p.PaymentMethod == PaymentMethod.InstaPay).Sum(p => p.AmountPaid),
                     Count = g.Count()
                 })
                 .FirstOrDefaultAsync(cancellationToken);

            if (stats == null)
                return (0, 0, 0, 0, 0);

            return (stats.Total, stats.Cash, stats.Insta, stats.Card, stats.Count);
        }

        public async Task<(decimal total, int count)> GetDoctorRevenueStatsAsync(int doctorId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            var stats = await context.Payments
                .AsNoTracking()
                .Where(p => p.Appointment.DoctorId == doctorId &&
                            p.PaymentStatus == PaymentStatus.Paid &&
                            p.PaymentDate >= from &&
                            p.PaymentDate <= to)
                .GroupBy(p => 1)
                .Select(g => new
                {
                    Total = g.Sum(p => p.AmountPaid),
                    Count = g.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (stats == null)
                return (0, 0);

            return (stats.Total, stats.Count);
        }
    }
}
