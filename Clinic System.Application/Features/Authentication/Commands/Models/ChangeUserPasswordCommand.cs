namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class ChangeUserPasswordCommand : IRequest<Response<bool>>
    {
        public string? UserId { get; set; } // يستخدمه الأدمن فقط
        public string? CurrentPassword { get; set; } // إلزامي للمستخدم العادي
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}