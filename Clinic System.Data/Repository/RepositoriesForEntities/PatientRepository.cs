namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Patient?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId, cancellationToken);
        }

        public async Task<IEnumerable<Patient?>> GetPatientsWithAppointmentsAsync(Expression<Func<Appointment, bool>> appointmentPredicate)
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

        public async Task<Patient?> GetPatientWithAppointmentsByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            return await context.Patients
                .AsNoTracking()
                .Include(d => d.Appointments.OrderBy(a => a.AppointmentDate))
                .FirstOrDefaultAsync(d => d.Id == Id);
        }

        public async Task<IEnumerable<Patient?>> GetPatientsByNameAsync(string fullName, CancellationToken cancellationToken = default)
        {
            return await context.Patients
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.FullName, $"%{fullName}%"))
                .OrderBy(d => d.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Patient?> GetPatientByPhoneAsync(string Phone, CancellationToken cancellationToken = default)
        {
            return await context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Phone == Phone, cancellationToken);
        }

        public async Task<string?> GetPatientUserIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await context.Patients
                .AsNoTracking()
                .Where(p => p.Id == patientId)
                .Select(p => p.ApplicationUserId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
