namespace Clinic_System.Application.Features.Authentication.Commands.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() 
        { 
            RuleFor(x => x.EmailOrUserName)
                .NotEmpty().WithMessage("Email or Username is required.")
                .MaximumLength(100).WithMessage("Email or Username must not exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .PasswordRule();
        }
    }
}
