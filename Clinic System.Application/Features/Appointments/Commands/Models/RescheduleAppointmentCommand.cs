namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class RescheduleAppointmentCommand : IRequest<Response<AppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}
