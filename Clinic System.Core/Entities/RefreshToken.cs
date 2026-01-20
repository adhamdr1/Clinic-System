namespace Clinic_System.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; } // لو لغينا التوكن لأسباب أمنية
        public bool IsActive => RevokedOn == null && !IsExpired;

        // ربط التوكن بالمستخدم
        public string ApplicationUserId { get; set; } = string.Empty;

         //public virtual ApplicationUser User { get; set; } = null!;
    }
}
