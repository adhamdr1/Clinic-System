namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status);
        Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date);
    }
}
