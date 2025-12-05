namespace Clinic_System.Core.Entities
{
    public class Appointment : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual DateTime AppointmentDate { get; set; }
        public virtual AppointmentStatus Status { get; set; }

        public virtual int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        public virtual int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;

        public virtual MedicalRecord? MedicalRecord { get; set; }
        public virtual Payment? Payment { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
