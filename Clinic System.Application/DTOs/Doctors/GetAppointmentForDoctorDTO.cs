namespace Clinic_System.Application.DTOs.Doctors
{
    public class GetAppointmentForDoctorDTO
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; } = null!;
        public int PatientId { get; set; }
        public string PatientFullName { get; set; } = null!;
    }
}
