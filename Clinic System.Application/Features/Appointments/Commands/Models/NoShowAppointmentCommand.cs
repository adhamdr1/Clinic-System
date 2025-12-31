namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class NoShowAppointmentCommand : IRequest<Response<CaneclledAndNoShowAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int DoctorId { get; set; }
    }
}
