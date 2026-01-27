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
        Task<(bool IsAuthenticated, bool IsEmailConfirmed, string Id, string UserName, string Email, List<string> Roles)> LoginAsync(string userNameOrEmail, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        string EncodeToken(string token);
        string DecodeToken(string encodedToken);
        Task<bool> ConfirmEmailAsync(string userId, string code);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<(bool Succeeded, string Error)> ResetPasswordAsync(string email, string decodedToken, string newPassword);
    }
}