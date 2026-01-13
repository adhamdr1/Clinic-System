namespace Clinic_System.Core.Entities
{
    public class MedicalRecord : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string Diagnosis { get; set; } = null!;
        public virtual string? AdditionalNotes { get; set; }
        public virtual string DescriptionOfTheVisit { get; set; } = null!;

        public virtual int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }


        public void Update(string? diagnosis,string? description,string? additionalNotes)
        {
            if (!string.IsNullOrWhiteSpace(diagnosis))
                this.Diagnosis = diagnosis;

            if (!string.IsNullOrWhiteSpace(description))
                this.DescriptionOfTheVisit = description;

            if (additionalNotes != null)
                this.AdditionalNotes = additionalNotes;
        }
    }
}
