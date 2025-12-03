namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PaymentRepository : GenericRepository<Payments>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payments>> GetDailyPaymentsAsync(DateTime date)
        {
            return await context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payments>> GetPaymentsWithAppointmentAsync(Expression<Func<Appointments, bool>> appointmentPredicate)
        {
            return await context.Payments
                .AsNoTracking()
                .Where(p => appointmentPredicate.Compile().Invoke(p.Appointment))
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
