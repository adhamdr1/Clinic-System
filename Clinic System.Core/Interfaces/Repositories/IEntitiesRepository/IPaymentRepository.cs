namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPaymentRepository : IGenericRepository<Payments>
    {
        Task<IEnumerable<Payments>> GetPaymentsWithAppointmentAsync(Expression<Func<Appointments, bool>> appointmentPredicate);

        Task<IEnumerable<Payments>> GetDailyPaymentsAsync(DateTime date);

        Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end);

        Task<IEnumerable<Payments>> GetUnpaidAppointmentsAsync();
    }
}
