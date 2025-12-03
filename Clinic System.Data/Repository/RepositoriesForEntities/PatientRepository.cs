namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PatientRepository : GenericRepository<Patients>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Patients?> GetPatientByUserIdAsync(string userId)
        {
            return await context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<Patients>> GetPatientsWithAppointmentsAsync(Expression<Func<Appointments, bool>> appointmentPredicate)
        {
            return await context.Patients
                .AsNoTracking()
                .Where(p => p.Appointments.AsQueryable().Any(appointmentPredicate))
                .ToListAsync();
        }
    }
}
