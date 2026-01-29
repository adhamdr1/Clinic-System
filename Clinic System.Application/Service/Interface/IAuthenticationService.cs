namespace Clinic_System.Application.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt ,string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(string userId, string userName,  string email, List<string> roles, List<Claim>? extraClaims = null);

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> RefreshTokenAsync(string accessToken, string refreshToken, List<Claim>? extraClaims = null);
    }
}
