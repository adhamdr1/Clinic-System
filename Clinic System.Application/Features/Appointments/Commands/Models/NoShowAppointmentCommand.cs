namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class NoShowAppointmentCommand : IRequest<Response<CaneclledAndNoShowAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
    }
}
