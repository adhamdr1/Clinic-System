namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class DoctorRepository : GenericRepository<Doctor> , IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Doctor?>> GetAvailableDoctorsAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            // الحل: إزالة Include غير الضروري واستخدام Subquery
            // هذا يمنع جلب كل الـ Appointments ثم الفلترة في Memory
            var busyDoctorIds = context.Appointments
                .Where(a => a.AppointmentDate == dateTime && 
                           a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.DoctorId)
                .Distinct();

            return await context.Doctors
                .AsNoTracking()
                .Where(d => !busyDoctorIds.Contains(d.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<string?> GetDoctorUserIdAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return await context.Doctors
                .AsNoTracking()
                .Where(d => d.Id == doctorId)
                .Select(d => d.ApplicationUserId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Doctor?> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<Doctor?>> GetDoctorsByNameAsync(string fullName, CancellationToken cancellationToken = default)
        {
            return await context.Doctors
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.FullName, $"%{fullName}%"))
                .OrderBy(d => d.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Doctor?>> GetDoctorsBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
        {
            // الحل: استخدام EF.Functions.Like مع wildcard للبحث Case-Insensitive
            // أو استخدام Collation مناسب في SQL Server
            // EF.Functions.Like مع % wildcard للبحث الجزئي
            return await context.Doctors
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.Specialization, $"%{specialization}%"))
                .OrderBy(d => d.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Doctor?>> GetDoctorsWithAppointmentsAsync(Expression<Func<Appointment, bool>> appointmentPredicate, CancellationToken cancellationToken = default)
        {
            // الحل: استخدام Join مع Appointments مباشرة بدلاً من AsQueryable()
            // هذا يضمن تنفيذ Query في SQL وليس في Memory
            var appointmentIds = context.Appointments
                .Where(appointmentPredicate)
                .Select(a => a.DoctorId)
                .Distinct();

            return await context.Doctors
                .AsNoTracking()
                .Where(d => appointmentIds.Contains(d.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            return await context.Doctors
                .AsNoTracking()
                .Include(d => d.Appointments.OrderBy(a => a.AppointmentDate))
                .FirstOrDefaultAsync(d => d.Id == Id);
        }
    }
}
