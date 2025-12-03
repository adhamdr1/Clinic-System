namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class DoctorRepository : GenericRepository<Doctors> , IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Doctors>> GetAvailableDoctorsAsync(DateTime dateTime)
        {
            return await context.Doctors
                .AsNoTracking()
                .Include(c=>c.Appointments)
                .Where(d => !d.Appointments.Any(a =>
                    a.AppointmentDate == dateTime &&
                    a.Status != AppointmentStatus.Cancelled))
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
            var normalizedSpecialization = specialization.ToLower();
            
            return await context.Doctors
                .AsNoTracking()
                .Where(d => d.Specialization.ToLower() == normalizedSpecialization)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctors>> GetDoctorsWithAppointmentsAsync(Expression<Func<Appointments, bool>> appointmentPredicate)
        {
            return await context.Doctors
                .AsNoTracking()
                .Where(d => d.Appointments.AsQueryable().Any(appointmentPredicate))
                .ToListAsync();
        }
    }
}
