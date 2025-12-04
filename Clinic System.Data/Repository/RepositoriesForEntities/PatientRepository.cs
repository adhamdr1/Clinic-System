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
            // الحل: استخدام Join مع Appointments مباشرة بدلاً من AsQueryable()
            // هذا يضمن تنفيذ Query في SQL وليس في Memory
            var patientIds = context.Appointments
                .Where(appointmentPredicate)
                .Select(a => a.PatientId)
                .Distinct();

            return await context.Patients
                .AsNoTracking()
                .Where(p => patientIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
