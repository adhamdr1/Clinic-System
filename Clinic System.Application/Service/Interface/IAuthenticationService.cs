namespace Clinic_System.Application.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt ,string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(string userId, string userName,  string email, List<string> roles);

        Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
