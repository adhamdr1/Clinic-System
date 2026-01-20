namespace Clinic_System.Application.DTOs.Authentications
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; }
        public string ExpiresAt { get; set; } = string.Empty;
    }
}
