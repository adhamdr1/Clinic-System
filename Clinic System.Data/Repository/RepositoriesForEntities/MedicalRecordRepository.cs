namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MedicalRecord?> GetMedicalRecordWithPrescriptionsAsync(int recordId)
        {
            return await context.MedicalRecords
                .AsNoTracking()
                .Include(mr => mr.Prescriptions)
                .FirstOrDefaultAsync(mr => mr.Id == recordId);
        }

        public async Task<IEnumerable<MedicalRecord>> GetPatientMedicalHistoryAsync(int patientId)
        {
            // الحل: إضافة Include للـ Appointment لتجنب N+1 Query Problem
            return await context.MedicalRecords
                .AsNoTracking()
                .Include(mr => mr.Appointment)
                .Where(mr => mr.Appointment.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetRecordsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await context.MedicalRecords
                .AsNoTracking()
                .Where(mr => mr.CreatedAt >= start && mr.CreatedAt <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetRecordsByDoctorAsync(int doctorId)
        {
            // الحل: إضافة Include للـ Appointment لتجنب N+1 Query Problem
            return await context.MedicalRecords
                .AsNoTracking()
                .Include(mr => mr.Appointment)
                .Where(mr => mr.Appointment.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> SearchByDiagnosisAsync(string diagnosis)
        {
            // الحل: استخدام EF.Functions.Like للبحث Case-Insensitive
            return await context.MedicalRecords
                .AsNoTracking()
                .Where(mr => EF.Functions.Like(mr.Diagnosis, $"%{diagnosis}%"))
                .ToListAsync();
        }
    }
}
