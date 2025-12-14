namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetAvailableSlotQuery : IRequest<Response<List<AvailableSlotDTO>>>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
