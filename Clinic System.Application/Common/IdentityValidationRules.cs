namespace Clinic_System.Application.Common
{
    public static class IdentityValidationRules
    {
        public static IRuleBuilderOptions<T, string> PasswordRule<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");
        }
    }
}
