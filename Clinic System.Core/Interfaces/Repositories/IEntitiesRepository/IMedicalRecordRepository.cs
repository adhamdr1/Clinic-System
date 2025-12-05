namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<MedicalRecord?> GetMedicalRecordWithPrescriptionsAsync(int recordId);

        Task<IEnumerable<MedicalRecord>> GetPatientMedicalHistoryAsync(int patientId);

        Task<IEnumerable<MedicalRecord>> SearchByDiagnosisAsync(string diagnosis);

        Task<IEnumerable<MedicalRecord>> GetRecordsByDoctorAsync(int doctorId);

        Task<IEnumerable<MedicalRecord>> GetRecordsByDateRangeAsync(DateTime start, DateTime end);
    }
}
