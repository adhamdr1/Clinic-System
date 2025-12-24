namespace Clinic_System.Application.Service.Interface
{
    public interface IPatientService
    {
        Task<List<Patient?>> GetPatientsListAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<Patient?>> GetPatientsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Patient?> GetPatientWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient?> GetPatientByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        Task<List<Patient?>> GetPatientListByNameAsync(string name, CancellationToken cancellationToken = default);
        Task CreatePatientAsync(Patient patient, CancellationToken cancellationToken = default);
        Task UpdatePatient(Patient patient, CancellationToken cancellationToken = default);
        Task SoftDeletePatient(Patient patient, CancellationToken cancellationToken = default);
        Task HardDeletePatient(Patient patient, CancellationToken cancellationToken = default);
    }
}
