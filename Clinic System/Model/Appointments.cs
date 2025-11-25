namespace Clinic_System.Model
{
    public class Appointments : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual DateTime AppointmentDate { get; set; }
        public virtual AppointmentStatus Status { get; set; }

        public virtual int PatientId { get; set; }
        public virtual Patients Patient { get; set; }

        public virtual int DoctorId { get; set; }
        public virtual Doctors Doctor { get; set; }

        public virtual MedicalRecords? MedicalRecord { get; set; }
        public virtual Payments? Payment { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
