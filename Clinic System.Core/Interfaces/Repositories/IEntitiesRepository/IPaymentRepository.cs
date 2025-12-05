namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsWithAppointmentAsync(Expression<Func<Appointment, bool>> appointmentPredicate);

        Task<IEnumerable<Payment>> GetDailyPaymentsAsync(DateTime date);

        Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end);

        //Task<IEnumerable<Payments>> GetUnpaidAppointmentsAsync();
    }
}
