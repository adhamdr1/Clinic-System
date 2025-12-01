namespace Clinic_System.Core.Entities
{
    public class Prescriptions : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string Dosage { get; set; } = null!;
        public virtual string MedicationName { get; set; } = null!;
        public virtual string? SpecialInstructions { get; set; }
        public virtual string Frequency { get; set; } = null!;
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual int MedicalRecordId { get; set; }
        public virtual MedicalRecords MedicalRecord { get; set; } = null!;

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
