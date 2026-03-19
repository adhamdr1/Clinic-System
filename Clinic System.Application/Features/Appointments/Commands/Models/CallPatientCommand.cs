namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class CallPatientCommand : IRequest<Response<string>>
    {
        public int AppointmentId { get; set; }
    }
}
