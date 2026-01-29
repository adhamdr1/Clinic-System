namespace Clinic_System.Application.Service.Interface
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        bool IsAuthenticated { get; }
        int? DoctorId { get; }
        int? PatientId { get; }
        Task<List<string>> GetCurrentUserRolesAsync();
    }
}
