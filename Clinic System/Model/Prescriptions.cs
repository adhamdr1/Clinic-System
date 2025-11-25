namespace Clinic_System.Model
{
    public class Prescriptions : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string Dosage { get; set; }
        public virtual string MedicationName { get; set; }
        public virtual string? SpecialInstructions { get; set; }
        public virtual string Frequency { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual int MedicalRecordId { get; set; }
        public virtual MedicalRecords MedicalRecord { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
