namespace Clinic_System.Application.DTOs.Payment
{
    public class PaymentDetailsDTO
    {
        public int PaymentId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } // مهم عشان التقرير يكون مقروء
        public string DoctorName { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDate { get; set; } // Formatted Date
        public string? Notes { get; set; }
    }
}
