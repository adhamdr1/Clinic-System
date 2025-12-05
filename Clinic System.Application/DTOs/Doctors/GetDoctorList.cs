namespace Clinic_System.Application.DTOs.Doctors
{
    public class GetDoctorList
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string CreatedAt { get; set; }
        public string Specialization { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
    }
}
