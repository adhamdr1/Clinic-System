namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment?> GetAppointmentWithDetailsAsync(int AppointmentId, CancellationToken cancellationToken = default);

        Task<(List<Appointment> Items, int TotalCount)> GetDoctorAppointmentsAsync(int doctorId,
            int pageNumber,
            int pageSize,
            DateTime? dateTime =null,
            CancellationToken cancellationToken = default);
        Task<(List<Appointment> Items, int TotalCount)> GetPatientAppointmentsAsync(int patientId,
            int pageNumber,
            int pageSize,
            DateTime? dateTime = null,
            CancellationToken cancellationToken = default);
        //Admin
        Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForAdminAsync(AppointmentStatus status,
            int pageNumber,
            int pageSize,
            DateTime? Start = null,
            DateTime? End = null,
            CancellationToken cancellationToken = default);
        //Doctor
        Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForDoctorAsync(AppointmentStatus status,
            int doctorId,
            int pageNumber,
            int pageSize,
            DateTime? Start = null,
            DateTime? End = null,
            CancellationToken cancellationToken = default);

        Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForDoctorAsync(int doctorId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForPatientAsync(int patientId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
        Task<Appointment?> GetNextUpcomingAppointmentAsync(int? doctorId, int? patientId, CancellationToken cancellationToken = default);
    }
}
