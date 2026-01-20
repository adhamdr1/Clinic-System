namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResult>>
    {
        public string AccessToken { get; set; } // (اختياري) أحياناً بنحتاجه للتحقق
        public string RefreshToken { get; set; } // ده الإجباري
    }
}
