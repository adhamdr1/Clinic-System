namespace Clinic_System.Application.DTOs.Doctors
{
    public class UpdateDoctorDTO
    {
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Specialization { get; set; } = null!;
    }
}