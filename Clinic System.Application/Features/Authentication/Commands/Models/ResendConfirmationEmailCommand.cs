namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class ResendConfirmationEmailCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string? BaseUrl { get; set; } 
    }
}
