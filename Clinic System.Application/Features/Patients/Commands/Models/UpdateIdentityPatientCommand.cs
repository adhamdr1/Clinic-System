namespace Clinic_System.Application.Features.Patients.Commands.Models
{
    public class UpdateIdentityPatientCommand : IRequest<Response<UserDTO>>
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
