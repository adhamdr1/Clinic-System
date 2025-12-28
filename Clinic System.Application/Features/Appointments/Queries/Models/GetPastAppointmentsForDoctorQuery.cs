namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetPastAppointmentsForDoctorQuery : IRequest<Response<PagedResult<DoctorAppointmentDTO>>>
    {
        public int DoctorId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
