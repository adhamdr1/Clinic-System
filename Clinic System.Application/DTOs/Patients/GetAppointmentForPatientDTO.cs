namespace Clinic_System.Application.DTOs.Patients
{
    public class GetAppointmentForPatientDTO
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; } = null!;
        public int DoctorId { get; set; }
        public string DoctorFullName { get; set; } = null!;
    }
}
