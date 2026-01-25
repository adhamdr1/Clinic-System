namespace Clinic_System.Application.Features.Patients.Commands.Models
{
    public class CreatePatientCommand : IRequest<Response<CreatePatientDTO>>
    {
        public string FullName { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;

        // Account Information
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

        [JsonIgnore]
        public string? BaseUrl { get; set; }
    }
}
