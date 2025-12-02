namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecords>
    {
        Task<MedicalRecords?> GetMedicalRecordWithPrescriptionsAsync(int recordId);

        Task<IEnumerable<MedicalRecords>> GetPatientMedicalHistoryAsync(int patientId);

        Task<IEnumerable<MedicalRecords>> SearchByDiagnosisAsync(string diagnosis);

        Task<IEnumerable<MedicalRecords>> GetRecordsByDoctorAsync(int doctorId);

        Task<IEnumerable<MedicalRecords>> GetRecordsByDateRangeAsync(DateTime start, DateTime end);
    }
}
