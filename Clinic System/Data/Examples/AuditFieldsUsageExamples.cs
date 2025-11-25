namespace Clinic_System.Data.Examples
{
    /// <summary>
    /// Examples of how CreatedAt and UpdatedAt work automatically
    /// </summary>
    public class AuditFieldsUsageExamples
    {
        private readonly AppDbContext _context;

        public AuditFieldsUsageExamples(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // Example 1: Create new entity (CreatedAt is set automatically)
        // ============================================
        public async Task CreatePatientExample()
        {
            var patient = new Patients
            {
                FullName = "Ahmed Mohamed",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "Cairo, Egypt",
                Phone = "01234567890",
                ApplicationUserId = "user-id-123"
            };

            // You DON'T need to set CreatedAt manually!
            // It will be set automatically when SaveChanges is called
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // After SaveChanges, CreatedAt will be set to current Egypt time (Cairo timezone)
            // patient.CreatedAt = GetEgyptTime() (automatically set)
            // patient.UpdatedAt = null (not set yet)
        }

        // ============================================
        // Example 2: Update entity (UpdatedAt is set automatically)
        // ============================================
        public async Task UpdatePatientExample(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                patient.FullName = "Ahmed Mohamed Updated";
                patient.Phone = "09876543210";

                // You DON'T need to set UpdatedAt manually!
                // It will be set automatically when SaveChanges is called
                await _context.SaveChangesAsync();

                // After SaveChanges, UpdatedAt will be set to current Egypt time (Cairo timezone)
                // patient.UpdatedAt = GetEgyptTime() (automatically set)
                // patient.CreatedAt remains unchanged (protected from modification)
            }
        }

        // ============================================
        // Example 3: Query by creation date
        // ============================================
        public async Task<List<Patients>> GetRecentPatients()
        {
            // Get patients created in the last 7 days (using Egypt time)
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            return await _context.Patients
                .Where(p => p.CreatedAt >= sevenDaysAgo)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // ============================================
        // Example 4: Query by last update date
        // ============================================
        public async Task<List<Patients>> GetRecentlyUpdatedPatients()
        {
            // Get patients updated in the last 24 hours (using Egypt time)
            var yesterday = DateTime.Now.AddDays(-1);
            return await _context.Patients
                .Where(p => p.UpdatedAt.HasValue && p.UpdatedAt >= yesterday)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();
        }

        // ============================================
        // Example 5: Check if entity was ever updated
        // ============================================
        public async Task<bool> WasPatientUpdated(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            return patient?.UpdatedAt != null;
        }

        // ============================================
        // Example 6: Get creation and update info
        // ============================================
        public async Task<(DateTime Created, DateTime? Updated)> GetPatientAuditInfo(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                return (patient.CreatedAt, patient.UpdatedAt);
            }
            throw new Exception("Patient not found");
        }

        // ============================================
        // Example 7: Sort by creation date
        // ============================================
        public async Task<List<Appointments>> GetAppointmentsByCreationDate()
        {
            return await _context.Appointments
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // ============================================
        // Example 8: Find entities created between dates
        // ============================================
        public async Task<List<Patients>> GetPatientsCreatedBetween(DateTime startDate, DateTime endDate)
        {
            return await _context.Patients
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToListAsync();
        }
    }
}

