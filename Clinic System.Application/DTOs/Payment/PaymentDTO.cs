namespace Clinic_System.Application.DTOs.Payment
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; } 
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDate { get; set; }
        public string? AdditionalNotes { get; set; }
        public string PaymentMethod { get; set; }
    }
}
