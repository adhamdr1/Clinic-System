namespace Clinic_System.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Doctors? Doctor { get; set; }
        public virtual Patients? Patient { get; set; }
    }
}

