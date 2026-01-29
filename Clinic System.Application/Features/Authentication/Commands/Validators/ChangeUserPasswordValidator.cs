namespace Clinic_System.Application.Features.Authentication.Commands.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        private readonly ICurrentUserService _currentUserService;

        public ChangeUserPasswordValidator(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            // 1. قاعدة الباسورد الحالي
            RuleFor(x => x.CurrentPassword)
                .MustAsync(async (currentPassword, ct) => {
                    var roles = await _currentUserService.GetCurrentUserRolesAsync();
                    return roles.Contains("Admin");
                })
                .When(x => string.IsNullOrEmpty(x.CurrentPassword))
                .WithMessage("Current password is required to change your password.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .PasswordRule();

            // 3. مطابقة التأكيد
            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("New password and confirmation do not match.");
        }
    }
}