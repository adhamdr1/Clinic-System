namespace Clinic_System.Application.Features.Doctors.Commands.Validators
{
    public class UpdateIdentityDoctorValidator : AbstractValidator<UpdateIdentityDoctorCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork unitOfWork;

        public UpdateIdentityDoctorValidator(IIdentityService identityService , IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            this.unitOfWork = unitOfWork;

            // تقسيم القواعد لتكون منظمة
            ApplyValidationsRules();
            ApplyCrossFieldRules();
            ApplyCustomValidationsRules();
        }
        private void ApplyValidationsRules()
        {
            // Email (Format Only)
            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format");

            RuleFor(x => x.UserName)
                .Matches(@"^(?=.*\d)[A-Za-z][A-Za-z0-9_]*$")
                .When(x => !string.IsNullOrEmpty(x.UserName))
                .WithMessage("Username must start with a letter and contain at least one number.");

            // Password Matching
            RuleFor(x => x.Password)
                .PasswordRule()
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password and Confirm Password do not match");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Current password is required when updating password.");
        }

        private void ApplyCrossFieldRules()
        {
            // شرط تحديث الايميل
            // شرط تحديث الايميل: إذا كان الإيميل مكتوباً، يجب أن يكون اليوزر مكتوباً أيضاً
            RuleFor(x => x.Email)
                .Must((command, email) => !(!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(command.UserName)))
                .WithMessage("To change Email, you must provide Username");

            // شرط تحديث اليوزر: إذا كان اليوزر مكتوباً، يجب أن يكون الإيميل مكتوباً أيضاً
            RuleFor(x => x.UserName)
                .Must((command, userName) => !(!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(command.Email)))
                .WithMessage("To change Username, you must provide Email");

            // شرط تحديث الباسورد
            RuleFor(x => x.Password)
                .Must((command, password) => !string.IsNullOrEmpty(password)
                                              ? (!string.IsNullOrEmpty(command.Email) || !string.IsNullOrEmpty(command.UserName))
                                              : true)
                .WithMessage("To change Password, you must provide Email or Username");
        }

        private  void ApplyCustomValidationsRules()
        {
            // 1. Check Email Uniqueness (Using Identity Service)
            RuleFor(x => x.Email)
                .MustAsync(async (command, email, cancellationToken) =>
                {
                    var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(command.Id);

                    if (doctor == null)
                        return true; // Doctor not found, skip validation

                    var oldEmail = await _identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken);

                    // Check if email exists
                    if (string.IsNullOrEmpty(email))
                        return true;

                    if (email == oldEmail)
                        return true;

                    bool exists = await _identityService.ExistingEmail(email);
                    // Return true if NOT exists (Valid), false if exists (Invalid)
                    return !exists;
                })
                .WithMessage("Email is already exists");

            // 2. Check UserName Uniqueness (Using Identity Service)
            RuleFor(x => x.UserName)
                .MustAsync(async (command, userName, cancellationToken) =>
                {
                    var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(command.Id);

                    if (doctor == null)
                        return true; // Doctor not found, skip validation

                    var oldUserName = await _identityService.GetUserNameAsync(doctor.ApplicationUserId, cancellationToken);


                    if (string.IsNullOrEmpty(userName))
                        return true;

                    if (userName == oldUserName)
                        return true;

                    bool exists = await _identityService.ExistingUserName(userName);
                    return !exists;
                })
                .WithMessage("Username is already exists");
        }
    }
}
