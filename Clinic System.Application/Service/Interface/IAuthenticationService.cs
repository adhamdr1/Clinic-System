namespace Clinic_System.Application.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<(string Token, DateTime ExpiresAt ,string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(string userId, string userName,  string email, List<string> roles);
    }
}
