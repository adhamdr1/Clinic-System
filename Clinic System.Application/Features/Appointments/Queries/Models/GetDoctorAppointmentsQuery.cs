namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetDoctorAppointmentsQuery : IRequest<Response<PagedResult<DoctorAppointmentDTO>>>
    {
        public int? DoctorId {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? DateTime { get; set; } = null; 
    }
}
