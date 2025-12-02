namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPatientRepository : IGenericRepository<Patients>
    {
        Task<Patients?> GetPatientByUserIdAsync(string userId);
        Task<IEnumerable<Patients>> GetPatientsWithAppointmentsAsync(
            Expression<Func<Appointments, bool>> appointmentPredicate);
    }
}
