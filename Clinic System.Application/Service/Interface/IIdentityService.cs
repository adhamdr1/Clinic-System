namespace Clinic_System.Application.Service.Interface
{
    public interface IIdentityService
    {
        Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default);
        Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default);
        Task<string> CreateUserAsync(string userName, string email, string password, string role, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserProfileAsync(string userId, string newEmail, string newUserName, string currentPassword, bool isAdmin, CancellationToken cancellationToken = default);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword, bool isAdmin, CancellationToken cancellationToken = default);
        Task<bool> IsEmailUniqueAsync(string email, string? excludeUserId = null, CancellationToken cancellationToken = default);
        Task<bool> IsUserNameUniqueAsync(string userName, string? excludeUserId = null, CancellationToken cancellationToken = default); Task<bool> SoftDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> HardDeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<(bool IsAuthenticated, bool IsEmailConfirmed, string Id, string UserName, string Email, List<string> Roles)> LoginAsync(string userNameOrEmail, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        string EncodeToken(string token);
        string DecodeToken(string encodedToken);
        Task<bool> ConfirmEmailAsync(string userId, string code);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<(bool Succeeded, string Error)> ResetPasswordAsync(string email, string decodedToken, string newPassword);
        Task<(string UserId, string UserName,string Role, string Token, string Error)> GenerateTokenForResendEmailConfirmationAsync(string email);
        Task<(string Email, string UserName)> GetUserEmailAndUserNameAsync(string userId, CancellationToken cancellationToken = default);
    }
}