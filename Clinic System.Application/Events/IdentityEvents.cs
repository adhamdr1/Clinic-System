namespace Clinic_System.Application.Events
{
    // 1. حدث تسجيل مستخدم جديد
    public record UserRegisteredEvent
    {
        public required string UserId { get; init; }
        public required string FullName { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string ConfirmationLink { get; init; }
        public required string UserRole { get; init; }
        public string? Specialty { get; init; }
    }

    // 2. حدث طلب تغيير الباسورد
    public record PasswordResetRequestedEvent
    {
        public required string Email { get; init; }
        public required string ResetLink { get; init; }
    }
}
