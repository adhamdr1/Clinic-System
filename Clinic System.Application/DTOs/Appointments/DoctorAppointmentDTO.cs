namespace Clinic_System.Application.DTOs.Appointments
{
    public class DoctorAppointmentDTO
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string PatientPhone { get; set; }
    }
}
