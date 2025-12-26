namespace Clinic_System.Application.DTOs.Appointments
{
    public class PatientAppointmentDTO
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhone { get; set; }
        public string Specialization { get; set; }
    }
}
