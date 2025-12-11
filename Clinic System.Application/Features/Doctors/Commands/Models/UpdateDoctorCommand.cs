namespace Clinic_System.Application.Features.Doctors.Commands.Models
{
    public class UpdateDoctorCommand : IRequest<Response<UpdateDoctorDTO>>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Specialization { get; set; } = null!;
    }
}
