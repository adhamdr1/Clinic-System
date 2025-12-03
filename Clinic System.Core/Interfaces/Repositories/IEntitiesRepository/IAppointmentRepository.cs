namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointments>
    {
        Task<IEnumerable<Appointments>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointments>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<Appointments>> GetAppointmentsByStatusAsync(AppointmentStatus status);
        Task<IEnumerable<Appointments>> GetAppointmentsInDateAsync(DateTime date);
    }
}
