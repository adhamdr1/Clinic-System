namespace Clinic_System.Application.Features.Authentication.Commands.Validators
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserProfileValidator(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;

            // 1. القواعد الأساسية (بتشتغل فقط لو الحقل مش null)
            RuleFor(x => x.NewEmail)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.NewEmail))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.NewUserName)
                .Matches(@"^(?=.*\d)[A-Za-z][A-Za-z0-9_]*$")
                .When(x => !string.IsNullOrEmpty(x.NewUserName))
                .WithMessage("Username must start with a letter and contain at least one number.");

            // 2. فحص تكرار الإيميل (الأهم)
            RuleFor(x => x.NewEmail)
                .MustAsync(async (command, newEmail, ct) =>
                {
                    if (string.IsNullOrEmpty(newEmail)) return true; // لو مبعتش إيميل، عدي

                    // هنجيب الـ ID بتاعنا عشان نقول للسيرفس "لو ده إيميلي أنا، متبلغيش عن تكرار"
                    var roles = await _currentUserService.GetCurrentUserRolesAsync();
                    string targetUserId = roles.Contains("Admin") && !string.IsNullOrEmpty(command.UserId)
                                          ? command.UserId
                                          : _currentUserService.UserId;

                    return await _identityService.IsEmailUniqueAsync(newEmail, targetUserId);
                })
                .When(x => !string.IsNullOrEmpty(x.NewEmail))
                .WithMessage("Email is already taken by another user.");

            RuleFor(x => x.NewUserName)
                .MustAsync(async (command, newUserName, ct) =>
                {
                    if (string.IsNullOrEmpty(newUserName)) return true; // لو مبعتش إيميل، عدي

                    // هنجيب الـ ID بتاعنا عشان نقول للسيرفس "لو ده إيميلي أنا، متبلغيش عن تكرار"
                    var roles = await _currentUserService.GetCurrentUserRolesAsync();
                    string targetUserId = roles.Contains("Admin") && !string.IsNullOrEmpty(command.UserId)
                                          ? command.UserId
                                          : _currentUserService.UserId;

                    return await _identityService.IsUserNameUniqueAsync(newUserName, targetUserId);
                })
                .When(x => !string.IsNullOrEmpty(x.NewUserName))
                .WithMessage("UserName is already taken by another user.");

            // 3. قاعدة الباسورد الحالي (مطلوب دايماً لليوزر العادي)
            RuleFor(x => x.CurrentPassword)
                .MustAsync(async (command, pass, ct) => {
                    var roles = await _currentUserService.GetCurrentUserRolesAsync();
                    return roles.Contains("Admin") || !string.IsNullOrEmpty(pass);
                })
                .WithMessage("Current password is required to confirm identity.");
        }
    }
}