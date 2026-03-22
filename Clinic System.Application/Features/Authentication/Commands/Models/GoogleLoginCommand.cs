namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class GoogleLoginCommand : IRequest<Response<AuthDTO>>
    {
        public string IdToken { get; set; }
    }
}
