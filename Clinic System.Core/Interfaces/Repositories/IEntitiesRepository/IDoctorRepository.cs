namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IDoctorRepository : IGenericRepository<Doctors>
    {
        Task<IEnumerable<Doctors>> GetDoctorsBySpecializationAsync(string specialization);
        Task<IEnumerable<Doctors>> GetAvailableDoctorsAsync(DateTime dateTime);
        Task<Doctors?> GetDoctorByUserIdAsync(string userId);

        Task<IEnumerable<Doctors>> GetDoctorsWithAppointmentsAsync(
            Expression<Func<Appointments, bool>> appointmentPredicate);
    }
}
