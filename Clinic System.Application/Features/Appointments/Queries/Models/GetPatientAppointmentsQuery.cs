namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetPatientAppointmentsQuery : IRequest<Response<PagedResult<PatientAppointmentDTO>>>
    {
        public int patientId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public DateTime? dateTime { get; set; } = null;
    }
}
