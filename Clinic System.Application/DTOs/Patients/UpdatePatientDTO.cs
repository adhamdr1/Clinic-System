namespace Clinic_System.Application.DTOs.Patients
{
    public class UpdatePatientDTO
    {
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}