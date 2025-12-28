namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetPastAppointmentsForPatientQuery : IRequest<Response<PagedResult<PatientAppointmentDTO>>>
    {
        public int PatientId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
