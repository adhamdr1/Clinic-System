namespace Clinic_System.Application.Service.Interface
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecord> CreateMedicalRecordAsync(
            Appointment appointment,
            string Diagnosis,
            string Description,
            List<PrescriptionDto> prescriptionDto,
            string? AdditionalNotes = null,
            CancellationToken cancellationToken = default);

        Task<MedicalRecord> GetRecordByIdAsync(int recordId, CancellationToken cancellationToken = default);
        
        Task<PagedResult<MedicalRecord>> GetPatientHistoryAsync(int pageNumber, int pageSize, int patientId, CancellationToken cancellationToken = default);
       
        Task<PagedResult<MedicalRecord>> GetRecordsByDateRangeAsync(DateTime start, DateTime end, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<PagedResult<MedicalRecord>> GetRecordsByDoctorIdAsync(int pageNumber, int pageSize, int patientId, CancellationToken cancellationToken = default);

        Task<MedicalRecord> UpdateAsync(int recordId, string? diagnosis, string? description, string? notes, CancellationToken cancellationToken = default);
    }
}
