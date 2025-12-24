namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor?>> GetDoctorsBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor?>> GetDoctorsByNameAsync(string FullName, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor?>> GetAvailableDoctorsAsync(DateTime dateTime, CancellationToken cancellationToken = default);
        Task<Doctor?> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor?>> GetDoctorsWithAppointmentsAsync(
            Expression<Func<Appointment, bool>> appointmentPredicate, CancellationToken cancellationToken = default);
        Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int Id, CancellationToken cancellationToken = default);
    }
}
