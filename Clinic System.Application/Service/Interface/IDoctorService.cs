namespace Clinic_System.Application.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor?>> GetDoctorsListAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<Doctor?>> GetDoctorsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<List<Doctor?>> GetDoctorsListBySpecializationAsync(string Specialization, CancellationToken cancellationToken = default);
        Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Doctor?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Doctor?>> GetDoctorsListByNameAsync(string name, CancellationToken cancellationToken = default);
        Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default);
        Task UpdateDoctor(Doctor doctor, CancellationToken cancellationToken = default);
        Task SoftDeleteDoctor(Doctor doctor,  CancellationToken cancellationToken = default);
        Task HardDeleteDoctor(Doctor doctor, CancellationToken cancellationToken = default);
    }
}
