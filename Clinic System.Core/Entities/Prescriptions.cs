namespace Clinic_System.Core.Entities
{
    public class Prescription : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string Dosage { get; set; } = null!;
        public virtual string MedicationName { get; set; } = null!;
        public virtual string? SpecialInstructions { get; set; }
        public virtual string Frequency { get; set; } = null!;
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual int MedicalRecordId { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; } = null!;

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }

        public void Update(string? medicationname, string? dosage,
        string? specialinstructions, string? frequency,
        DateTime? startDate, DateTime? endDate)
        {
            // بنستخدم ! قبل string.IsNullOrEmpty عشان نحدث لو فيه قيمة فعلاً
            if (!string.IsNullOrEmpty(medicationname))
                this.MedicationName = medicationname;

            if (!string.IsNullOrEmpty(dosage))
                this.Dosage = dosage;

            if (specialinstructions != null) // مسموح يكون string فاضي لو عايز يمسح الملحوظة
                this.SpecialInstructions = specialinstructions;

            if (!string.IsNullOrEmpty(frequency))
                this.Frequency = frequency;

            if (startDate.HasValue)
                this.StartDate = startDate.Value;

            if (endDate.HasValue)
                this.EndDate = endDate.Value;
        }


        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeletedAt = DateTime.Now;
        }
    }
}
