namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class BookAppointmentCommand : IRequest<Response<CreateAppointmentDTO>>
    {
        [JsonIgnore]
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}
