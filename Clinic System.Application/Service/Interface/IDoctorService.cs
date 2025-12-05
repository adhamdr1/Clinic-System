namespace Clinic_System.Application.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorsListAsync();
    }
}
