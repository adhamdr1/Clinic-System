namespace Clinic_System.Application.Features.Doctors.Commands.Models
{
    public class UpdateDoctorCommand : IRequest<Response<UpdateDoctorDTO>>
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Specialization { get; set; }
    }
}
