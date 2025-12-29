namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class CancelAppointmentCommand : IRequest<Response<CaneclledAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int PatientId { get; set; }
    }
}
