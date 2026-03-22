namespace Clinic_System.Application.DTOs.Authentications
{
    public class AuthDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
        public bool RequiresCompletion { get; set; }
        public string GoogleEmail { get; set; } = string.Empty;
        public string GoogleName { get; set; } = string.Empty;
    }
}
