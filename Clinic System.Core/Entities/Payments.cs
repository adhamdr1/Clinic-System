using System.Net.NetworkInformation;

namespace Clinic_System.Core.Entities
{
    public class Payment : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual decimal AmountPaid { get; set; }
        public virtual string? AdditionalNotes { get; set; }
        public virtual DateTime? PaymentDate { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public virtual int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }

        public void MarkAsPaid(PaymentMethod method, string? additionalNotes = null, decimal? amount = null)
        {
            if (PaymentStatus == PaymentStatus.Paid)
                throw new InvalidOperationException("Payment already paid.");

            PaymentStatus = PaymentStatus.Paid;

            AdditionalNotes = additionalNotes ?? AdditionalNotes;
            AmountPaid = amount ?? AmountPaid;
            PaymentMethod = method;
            PaymentDate = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void MarkAsFailed(string? reason = null)
        {
            PaymentStatus = PaymentStatus.Failed;
            AdditionalNotes = reason;
            UpdatedAt = DateTime.Now;
        }
    }
}
