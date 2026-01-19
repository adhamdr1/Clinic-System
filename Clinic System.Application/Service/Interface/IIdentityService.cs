namespace Clinic_System.Application.Service.Interface
{
    public interface IIdentityService
    {
        Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default);
        Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> ExistingEmail(string Email);
        Task<bool> ExistingUserName(string UserName);
        Task<string> CreateUserAsync(string userName, string email, string password, string role, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmailUserAsync(string userId, string newEmail, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserNameAsync(string userId, string newUserName, CancellationToken cancellationToken = default);
        Task<bool> UpdatePasswordUserAsync(string userId, string newpassword,string currentPassword,  CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> HardDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<(bool IsAuthenticated, string Id, string UserName, string Email, List<string> Roles)> LoginAsync(string userNameOrEmail, string password);
    }
}

