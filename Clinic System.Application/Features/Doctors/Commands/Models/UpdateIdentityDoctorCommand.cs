namespace Clinic_System.Application.Features.Doctors.Commands.Models
{
    public class UpdateIdentityDoctorCommand : IRequest<Response<UserDTO>>
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
