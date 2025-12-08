namespace Clinic_System.Application.Service.Interface
{
    public interface IIdentityService
    {
        Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default);
        Task<string> CreateUserAsync(string userName, string email, string password, string role, CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> HardDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}

