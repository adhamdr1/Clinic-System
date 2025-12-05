namespace Clinic_System.Core.Entities
{
    public class Payment : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual decimal AmountPaid { get; set; }
        public virtual string? AdditionalNotes { get; set; }
        public virtual DateTime PaymentDate { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
