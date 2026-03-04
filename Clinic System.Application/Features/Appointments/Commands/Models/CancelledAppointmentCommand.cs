namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class CancelAppointmentCommand : IRequest<Response<CaneclledAndNoShowAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
    }
}
