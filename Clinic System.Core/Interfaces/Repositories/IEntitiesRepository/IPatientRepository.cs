namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetPatientByUserIdAsync(string userId);
        Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync(
            Expression<Func<Appointment, bool>> appointmentPredicate);
    }
}
