namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<MedicalRecord?> GetMedicalRecordDetailsAsync(int recordId , CancellationToken cancellationToken = default);
        Task<MedicalRecord?> GetMedicalRecordForUpdateAsync(int recordId, CancellationToken cancellationToken = default);
        Task<(List<MedicalRecord> Items, int TotalCount)> GetPatientMedicalHistoryAsync(int patientId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<(List<MedicalRecord> Items, int TotalCount)> GetRecordsByDoctorAsync(int doctorId, int pageNumber, int pageSize, DateTime? start, DateTime? end, CancellationToken cancellationToken = default);
    }
}
