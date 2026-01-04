namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class ConfirmAppointmentCommand : IRequest<Response<ConfirmAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int PatientId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod method { get; set; }
        public decimal? amount { get; set; } = null;
        public string? Notes { get; set; }
    }
}