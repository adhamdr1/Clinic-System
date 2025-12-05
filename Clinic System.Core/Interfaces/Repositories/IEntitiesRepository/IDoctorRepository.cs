namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialization);
        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime dateTime);
        Task<Doctor?> GetDoctorByUserIdAsync(string userId);

        Task<IEnumerable<Doctor>> GetDoctorsWithAppointmentsAsync(
            Expression<Func<Appointment, bool>> appointmentPredicate);
    }
}
