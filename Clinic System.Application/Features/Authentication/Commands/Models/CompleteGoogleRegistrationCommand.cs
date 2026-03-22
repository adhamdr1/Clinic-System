namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class CompleteGoogleRegistrationCommand : IRequest<Response<AuthDTO>>
    {
        // 1. البيانات اللي جاية من جوجل (والفرونت إند بيبعتهالنا تاني)
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        // 2. البيانات الجديدة اللي المريض كملها في الشاشة
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;

    }
}
