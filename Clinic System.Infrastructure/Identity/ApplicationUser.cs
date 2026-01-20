namespace Clinic_System.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new();
        public virtual Doctor? Doctor { get; set; }
        public virtual Patient? Patient { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }
    }
}

