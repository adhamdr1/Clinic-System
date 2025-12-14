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
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();
        }
    }
}
