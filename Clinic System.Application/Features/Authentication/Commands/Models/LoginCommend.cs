namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class LoginCommand : IRequest<Response<LoginResponseDTO>>
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}
