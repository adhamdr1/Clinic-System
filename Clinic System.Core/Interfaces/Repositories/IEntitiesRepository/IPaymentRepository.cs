namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<(List<Payment> Items, int TotalCount)> GetFilteredPaymentsAsync(
        int? doctorId,
        int? patientId,
        DateTime? fromDate,
        DateTime? toDate,
        PaymentStatus? status,
        PaymentMethod? method,
        int pageNumber,
        int pageSize);

        Task<Payment> GetPaymentDetailsByIdAsync(int id);
        Task<(decimal total, decimal cash, decimal insta, decimal card, int count)> GetDailyTotalsAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentByAppointmentIdAsync(int appointmentId);

        Task<(decimal total, int count)> GetDoctorRevenueStatsAsync(int doctorId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}
