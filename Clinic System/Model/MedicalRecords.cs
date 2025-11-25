namespace Clinic_System.Model
{
    public class MedicalRecords : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string Diagnosis { get; set; }
        public virtual string? AdditionalNotes { get; set; }
        public virtual string DescriptionOfTheVisit { get; set; }

        public virtual int AppointmentId { get; set; }
        public virtual Appointments Appointment { get; set; }

        public virtual ICollection<Prescriptions> Prescriptions { get; set; } = new List<Prescriptions>();

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }

}
