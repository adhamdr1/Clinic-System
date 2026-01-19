namespace Clinic_System.Infrastructure.Authentication.Models
{
    public class JwtSettings
    {
        public string SecritKey { get; set; } = string.Empty;
        public string AudienceIP { get; set; } = string.Empty;
        public string IssuerIP { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; } = 60; // ضيف دي عشان نتحكم في الوقت بسهولة
    }
}
