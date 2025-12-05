namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetDailyPaymentsAsync(DateTime date)
        {
            // الحل: استخدام Date Range بدلاً من .Date في Query لتحسين الأداء
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            
            return await context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate < endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsWithAppointmentAsync(Expression<Func<Appointment, bool>> appointmentPredicate)
        {
            // الحل: استخدام Join بدلاً من Compile().Invoke() لضمان تنفيذ Query في SQL
            return await context.Payments
                .AsNoTracking()
                .Include(p => p.Appointment)
                .Where(p => context.Appointments
                    .Where(appointmentPredicate)
                    .Select(a => a.Id)
                    .Contains(p.AppointmentId))
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end)
        {
            return await context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
                .SumAsync(p => p.AmountPaid);
        }

        //public async Task<IEnumerable<Payments>> GetUnpaidAppointmentsAsync()
        //{
        //    return await context.Payments
        //        .AsNoTracking()
        //        .Where(p => p.Appointment.Status == PaymentStatus.Unpaid)
        //        .ToListAsync();
        //}
    }
}
