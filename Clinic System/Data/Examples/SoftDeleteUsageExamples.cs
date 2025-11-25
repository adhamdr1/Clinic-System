namespace Clinic_System.Data.Examples
{
    /// <summary>
    /// Examples of how to use Soft Delete in your application
    /// </summary>
    public class SoftDeleteUsageExamples
    {
        private readonly AppDbContext _context;

        public SoftDeleteUsageExamples(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // Example 1: Normal Query (automatically excludes deleted records)
        // ============================================
        public async Task<List<Patients>> GetAllActivePatients()
        {
            // This will automatically exclude deleted patients (IsDeleted = false)
            return await _context.Set<Patients>().ToListAsync();
        }

        // ============================================
        // Example 2: Soft Delete a record
        // ============================================
        public async Task SoftDeletePatient(int patientId)
        {
            var patient = await _context.Set<Patients>().FindAsync(patientId);
            if (patient != null)
            {
                // Method 1: Using Remove() - automatically converted to soft delete
                _context.Set<Patients>().Remove(patient);
                await _context.SaveChangesAsync();

                // Method 2: Using Extension Method
                // _context.Set<Patients>().SoftDelete(patient);
                // await _context.SaveChangesAsync();

                // Method 3: Manual (using Egypt time)
                // patient.IsDeleted = true;
                // patient.DeletedAt = EgyptTimeHelper.GetEgyptTime();
                // await _context.SaveChangesAsync();
            }
        }

        // ============================================
        // Example 3: Get only deleted records
        // ============================================
        public async Task<List<Patients>> GetDeletedPatients()
        {
            return await _context.Set<Patients>()
                .OnlyDeleted()
                .ToListAsync();
        }

        // ============================================
        // Example 4: Include deleted records in query
        // ============================================
        public async Task<List<Patients>> GetAllPatientsIncludingDeleted()
        {
            return await _context.Set<Patients>()
                .IncludeDeleted()
                .ToListAsync();
        }

        // ============================================
        // Example 5: Restore a deleted record
        // ============================================
        public async Task RestorePatient(int patientId)
        {
            var patient = await _context.Set<Patients>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == patientId && p.IsDeleted);

            if (patient != null)
            {
                _context.Set<Patients>().Restore(patient);
                await _context.SaveChangesAsync();
            }
        }

        // ============================================
        // Example 6: Hard Delete (permanent delete)
        // ============================================
        public async Task HardDeletePatient(int patientId)
        {
            var patient = await _context.Set<Patients>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient != null)
            {
                _context.Set<Patients>().HardDelete(patient);
                await _context.SaveChangesAsync();
            }
        }

        // ============================================
        // Example 7: Query with related entities
        // ============================================
        public async Task<Patients?> GetPatientWithAppointments(int patientId)
        {
            // Related entities (Appointments) will also respect soft delete
            return await _context.Set<Patients>()
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }

        // ============================================
        // Example 8: Count deleted vs non-deleted
        // ============================================
        public async Task<(int Active, int Deleted)> GetPatientCounts()
        {
            var active = await _context.Set<Patients>().CountAsync();
            var deleted = await _context.Set<Patients>().OnlyDeleted().CountAsync();
            return (active, deleted);
        }
    }
}

