namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetNextUpcomingAppointmentAsync(int? doctorId, int? patientId, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments.AsNoTracking().Where(a => a.AppointmentDate > DateTime.Now);
            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }
            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }
            return await query.OrderBy(a => a.AppointmentDate).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
