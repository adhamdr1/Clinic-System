namespace Clinic_System.Application.Features.Appointments.Commands.Models
{
    public class CompleteAppointmentCommand : IRequest<Response<CompleteAppointmentDTO>>
    {
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public string? AdditionalNotes { get; set; } = null!;
        public List<PrescriptionDto> Medicines { get; set; } = new();
    }
}
