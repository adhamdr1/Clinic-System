namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class SendResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string? BaseUrl { get; set; } // URL of the front-end page for resetting password
    }
}