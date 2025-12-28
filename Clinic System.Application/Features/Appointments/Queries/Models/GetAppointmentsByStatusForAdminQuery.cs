namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetAppointmentsByStatusForAdminQuery : IRequest<Response<PagedResult<AppointmentsByStatusForAdminDTO>>>
    {
        public AppointmentStatus Status {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? Start { get; set; } = null; 
        public DateTime? End { get; set; } = null;
    }
}
