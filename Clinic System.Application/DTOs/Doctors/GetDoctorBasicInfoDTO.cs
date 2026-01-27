namespace Clinic_System.Application.DTOs.Doctors
{
    public class GetDoctorBasicInfoDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Specialization { get; set; } = null!;
    }
}
