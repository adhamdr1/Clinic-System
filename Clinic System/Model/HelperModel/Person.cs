namespace Clinic_System.Model.HelperModel
{
    public abstract class Person : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phone { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
