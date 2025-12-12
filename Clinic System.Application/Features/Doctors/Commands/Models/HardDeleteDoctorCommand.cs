namespace Clinic_System.Application.Features.Doctors.Commands.Models
{
    public class HardDeleteDoctorCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
