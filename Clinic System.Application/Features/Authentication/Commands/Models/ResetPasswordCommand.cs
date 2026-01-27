namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Code { get; set; } 
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
