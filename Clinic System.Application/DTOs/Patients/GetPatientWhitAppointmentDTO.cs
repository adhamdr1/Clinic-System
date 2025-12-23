namespace Clinic_System.Application.DTOs.Patients
{
    public class GetPatientWhitAppointmentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string CreatedAt { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public string Email { get; set; }
        public int TotalAppointments => Appointments.Count;
        public List<GetAppointmentForDoctorDTO> Appointments { get; set; } = new();
    }
}
