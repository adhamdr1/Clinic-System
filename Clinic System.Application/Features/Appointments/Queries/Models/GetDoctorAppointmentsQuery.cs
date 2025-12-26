namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetDoctorAppointmentsQuery : IRequest<Response<PagedResult<DoctorAppointmentDTO>>>
    {
        public int doctorId {  get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public DateTime? dateTime { get; set; } = null; 
    }
}
