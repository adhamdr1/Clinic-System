namespace Clinic_System.Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
        public virtual Doctors Doctor { get; set; }
        public virtual Patients Patient { get; set; }
    }
}
