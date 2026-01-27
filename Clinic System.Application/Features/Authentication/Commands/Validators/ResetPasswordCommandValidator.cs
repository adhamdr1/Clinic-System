namespace Clinic_System.Application.Features.Authentication.Commands.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            // التحقق من الإيميل
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            // التحقق من الكود (التوكن)
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required");

            // التحقق من الباسورد (باستخدام الرولز بتاعتك)
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .PasswordRule(); // دي الميثود اللي أنت عاملها

            // التحقق من التطابق
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Password and Confirm Password do not match");
        }
    }
}
