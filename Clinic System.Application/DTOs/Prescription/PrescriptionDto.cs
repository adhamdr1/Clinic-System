namespace Clinic_System.Application.DTOs.Prescription
{
    public class PrescriptionDto
    {
        public string MedicationName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string? SpecialInstructions { get; set; }
        public string Frequency { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
