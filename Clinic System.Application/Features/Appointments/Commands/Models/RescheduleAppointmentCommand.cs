namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class RescheduleAppointmentCommand : IRequest<Response<CreateAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}
