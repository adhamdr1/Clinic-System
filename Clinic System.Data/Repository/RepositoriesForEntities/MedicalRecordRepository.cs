namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MedicalRecord?> GetMedicalRecordDetailsAsync(int recordId, CancellationToken cancellationToken = default)
        {
            return await context.MedicalRecords
                .AsNoTracking()
                .Include(mr => mr.Prescriptions) 
                .Include(mr => mr.Appointment)   
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(mr => mr.Id == recordId , cancellationToken);
        }

        public async Task<MedicalRecord?> GetMedicalRecordForUpdateAsync(int recordId, CancellationToken cancellationToken = default)
        {
            return await context.MedicalRecords
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(mr => mr.Id == recordId, cancellationToken);
        }

        public async Task<(List<MedicalRecord> Items, int TotalCount)> GetPatientMedicalHistoryAsync(int patientId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = context.MedicalRecords
                .AsNoTracking()
                .Where(mr => mr.Appointment.PatientId == patientId)
                .OrderByDescending(mr => mr.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<MedicalRecord> Items, int TotalCount)> GetRecordsByDoctorAsync(int doctorId, int pageNumber, int pageSize, DateTime? start, DateTime? end, CancellationToken cancellationToken = default)
        {
            var query = context.MedicalRecords
                .AsNoTracking()
                .Where(mr => mr.Appointment.DoctorId == doctorId);

            if (start.HasValue)
            {
                query = query.Where(mr => mr.CreatedAt >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(mr => mr.CreatedAt <= end.Value);
            }

            query = query.OrderByDescending(mr => mr.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync(cancellationToken);
            return (items, totalCount);
        }
    }
}
