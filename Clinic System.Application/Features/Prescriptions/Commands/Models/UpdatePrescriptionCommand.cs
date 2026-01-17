namespace Clinic_System.Application.Features.Prescriptions.Commands.Models
{
    public class UpdatePrescriptionCommand : IRequest<Response<PrescriptionDto>>
    {
        public int PrescriptionId { get; set; }
        public string? MedicationName { get; set; } = null!;
        public string? Dosage { get; set; } = null!;
        public string? SpecialInstructions { get; set; }
        public string? Frequency { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}