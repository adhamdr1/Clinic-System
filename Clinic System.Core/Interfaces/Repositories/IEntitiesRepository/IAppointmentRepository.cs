namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
    }
}
