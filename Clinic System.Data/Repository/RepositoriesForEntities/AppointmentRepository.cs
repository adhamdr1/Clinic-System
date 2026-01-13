using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
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
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.AppointmentDate > DateTime.Now);

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

        public async Task<(List<Appointment> Items, int TotalCount)> GetDoctorAppointmentsAsync(int doctorId, int pageNumber, int pageSize, DateTime? dateTime = null, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId);

            if (dateTime.HasValue)
            {
                var startDate = dateTime.Value.Date;
                var endDate = startDate.AddDays(1);

                query = query.Where(a => a.AppointmentDate >= startDate
                && a.AppointmentDate < endDate);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<Appointment> Items, int TotalCount)> GetPatientAppointmentsAsync(int patientId, int pageNumber, int pageSize, DateTime? dateTime = null, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId);
            if (dateTime.HasValue)
            {
                var startDate = dateTime.Value.Date;
                var endDate = startDate.AddDays(1);
                query = query.Where(a => a.AppointmentDate >= startDate
                && a.AppointmentDate < endDate);
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Doctor)
                .ToListAsync(cancellationToken);
            return (items, totalCount);
        }

        public async Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForAdminAsync(AppointmentStatus status, int pageNumber, int pageSize, DateTime? Start = null, DateTime? End = null, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status);

            query = FilterAppointments(query, Start, End);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForDoctorAsync(AppointmentStatus status, int doctorId, int pageNumber, int pageSize, DateTime? Start = null, DateTime? End = null, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.Status == status && a.DoctorId == doctorId);

            query = FilterAppointments(query, Start, End);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        private IQueryable<Appointment> FilterAppointments(IQueryable<Appointment> query, DateTime? start, DateTime? end)
        {
            if (start.HasValue)
                query = query.Where(a => a.AppointmentDate >= start.Value.Date);
            if (end.HasValue)
                query = query.Where(a => a.AppointmentDate <= end.Value.Date.AddDays(1).AddTicks(-1));
            return query;
        }

        public async Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForDoctorAsync(int doctorId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate < DateTime.Now);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForPatientAsync(int patientId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId && a.AppointmentDate < DateTime.Now);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.AppointmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(int AppointmentId, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalRecord) 
                .ThenInclude(mr => mr.Prescriptions)
                .FirstOrDefaultAsync(a => a.Id == AppointmentId, cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetPendingOverdueAppointmentsAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            return await context.Appointments
                .Include(a => a.Payment)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Status == AppointmentStatus.Pending && a.AppointmentDate < date)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<AppointmentStatus, int>> GetAppointmentsCountByStatusAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            var query = context.Appointments
                .Where(a => a.AppointmentDate >= start && a.AppointmentDate <= end);

            // نرجع Count لكل حالة على شكل Dictionary
            var result = await query
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);

            return result;
        }
    }
}
