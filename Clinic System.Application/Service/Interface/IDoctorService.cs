namespace Clinic_System.Application.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorsListAsync();
        //Task<PagedResult<Doctor>> GetDoctorsListPagingAsync(int pageNumber, int pageSize);

    }
}
