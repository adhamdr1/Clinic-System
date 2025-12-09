namespace Clinic_System.Application.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorsListAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<Doctor>> GetDoctorsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default);
        Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default);
    }
}
