namespace Clinic_System.Application.Features.Doctors.Commands.Validators
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDoctorValidator(IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _unitOfWork = unitOfWork;

            // تقسيم القواعد لتكون منظمة
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            // Name
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Doctor Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            // Address & Specialization
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required");

            // Phone (Format Only)
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Phone number must contain 10–15 digits (numbers only, optional +)");

            // Email (Format Only)
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Matches(@"^(?=.*\d)[A-Za-z][A-Za-z0-9_]*$")
                .WithMessage("Username must start with a letter and contain at least one number.");


            // Date of Birth
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past");

            // Password Matching
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .PasswordRule();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password and Confirm Password do not match");
        }

        public void ApplyCustomValidationsRules()
        {
            // 1. Check Email Uniqueness (Using Identity Service)
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) =>
                {
                    // Check if email exists
                    bool exists = await _identityService.ExistingEmail(email);
                    // Return true if NOT exists (Valid), false if exists (Invalid)
                    return !exists;
                })
                .WithMessage("Email is already exists");

            // 2. Check UserName Uniqueness (Using Identity Service)
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MustAsync(async (userName, cancellationToken) =>
                {
                    bool exists = await _identityService.ExistingUserName(userName);
                    return !exists;
                })
                .WithMessage("Username is already exists");

            // 3. Check Phone Uniqueness (Using UnitOfWork -> Doctor Repo)
            RuleFor(x => x.Phone)
                .MustAsync(async (phone, cancellationToken) =>
                {
                    // Search in Doctors table
                    // تأكد أن لديك ميثود FindAsync أو استخدم AnyAsync لو متاحة
                    var existingDoctors = await _unitOfWork.DoctorsRepository.FindAsync(d => d.Phone == phone);
                    var existingPatiants = await _unitOfWork.PatientsRepository.FindAsync(d => d.Phone == phone);


                    // If count is 0, then phone is unique (Valid)
                    return !existingDoctors.Any() && !existingPatiants.Any();
                })
                .WithMessage("Phone number is already exists");
        }
    }
}
