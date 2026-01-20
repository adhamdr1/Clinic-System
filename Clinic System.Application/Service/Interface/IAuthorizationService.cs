namespace Clinic_System.Application.Service.Interface
{
    public interface IAuthorizationService
    {
        Task<string> AddRoleAsync(string userId, string roleName);

        Task<bool> IsRoleExistAsync(string roleName);
    }
}
