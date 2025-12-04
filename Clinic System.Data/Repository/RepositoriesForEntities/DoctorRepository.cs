namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class DoctorRepository : GenericRepository<Doctors> , IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Doctors>> GetAvailableDoctorsAsync(DateTime dateTime)
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
                .ToListAsync();
        }

        public async Task<Doctors?> GetDoctorByUserIdAsync(string userId)
        {
            return await context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<Doctors>> GetDoctorsBySpecializationAsync(string specialization)
        {
            // الحل: استخدام EF.Functions.Like مع wildcard للبحث Case-Insensitive
            // أو استخدام Collation مناسب في SQL Server
            // EF.Functions.Like مع % wildcard للبحث الجزئي
            return await context.Doctors
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.Specialization, $"%{specialization}%"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctors>> GetDoctorsWithAppointmentsAsync(Expression<Func<Appointments, bool>> appointmentPredicate)
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
                .ToListAsync();
        }
    }
}
