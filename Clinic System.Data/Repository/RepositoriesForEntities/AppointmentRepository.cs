
namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsInDateAsync(DateTime date)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .ToListAsync();
        }
    }
}
