namespace Clinic_System.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Doctor? Doctor { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}

