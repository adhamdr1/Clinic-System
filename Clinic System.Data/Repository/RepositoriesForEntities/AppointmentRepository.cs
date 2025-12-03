
namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class AppointmentRepository : GenericRepository<Appointments>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointments>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointments>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointments>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointments>> GetAppointmentsInDateAsync(DateTime date)
        {
            return await context.Appointments
                .AsNoTracking()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .ToListAsync();
        }
    }
}
