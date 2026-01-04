namespace Clinic_System.Application.Features.Appointments.Queries.Models
{
    public class GetAdminAppointmentsStatsQuery : IRequest<AppointmentStatsDto>
    {
        public DateTime? StartDate { get; set; } // ممكن يكون بداية الأسبوع/اليوم
        public DateTime? EndDate { get; set; }   // نهاية الأسبوع/اليوم
    }
}
