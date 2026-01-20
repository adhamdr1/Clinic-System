namespace Clinic_System.Application.DTOs.Authentications
{
    public class LoginResponseDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new(); 
    }
}
