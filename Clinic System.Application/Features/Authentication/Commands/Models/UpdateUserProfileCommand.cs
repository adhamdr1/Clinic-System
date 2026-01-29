namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class UpdateUserProfileCommand : IRequest<Response<bool>>
    {
        public string? UserId { get; set; } // للأدمن فقط
        public string? NewUserName { get; set; }
        public string? NewEmail { get; set; }
        public string? CurrentPassword { get; set; } // إلزامي لغير الأدمن
    }
}
