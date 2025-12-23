namespace Clinic_System.Application.Features.Patients.Commands.Validators
{
    public class UpdateIdentityPatientValidator : AbstractValidator<UpdateIdentityPatientCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork unitOfWork;

        public UpdateIdentityPatientValidator(IIdentityService identityService , IUnitOfWork unitOfWork)
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
            // 1. فحص الإيميل الذكي
            RuleFor(x => x.Email)
                .MustAsync(async (command, email, cancellationToken) =>
                {
                    var patient = await unitOfWork.PatientsRepository.GetByIdAsync(command.Id);
                    if (patient == null) return true;

                    var oldEmail = await _identityService.GetUserEmailAsync(patient.ApplicationUserId, cancellationToken);

                    // لو الإيميل فاضي أو هو هو القديم، خلاص مش محتاجين يوزر نيم
                    if (string.IsNullOrWhiteSpace(email) || email == oldEmail)
                        return true;

                    // لو الإيميل جديد (مختلف)، لازم نتأكد إنه بعت اليوزر نيم (سواء قديم أو جديد)
                    if (string.IsNullOrWhiteSpace(command.UserName))
                        return false; // هنا هيطلع رسالة الخطأ إن اليوزر نيم مطلوب

                    // نكمّل فحص هل الإيميل موجود لمستخدم تاني ولا لأ
                    bool exists = await _identityService.ExistingEmail(email);
                    return !exists;
                })
                .WithMessage("To change Email, you must provide Username AND Email must be unique.");

            // 2. فحص اليوزر نيم الذكي
            RuleFor(x => x.UserName)
                .MustAsync(async (command, userName, cancellationToken) =>
                {
                    var patient = await unitOfWork.PatientsRepository.GetByIdAsync(command.Id);
                    if (patient == null) return true;

                    var oldUserName = await _identityService.GetUserNameAsync(patient.ApplicationUserId, cancellationToken);

                    if (string.IsNullOrWhiteSpace(userName) || userName == oldUserName)
                        return true;

                    if (string.IsNullOrWhiteSpace(command.Email))
                        return false;

                    bool exists = await _identityService.ExistingUserName(userName);
                    return !exists;
                })
                .WithMessage("To change Username, you must provide Email AND Username must be unique.");

            // 3. قاعدة الباسورد الصارمة: لو بتبعث باسورد، لازم تبعث معاه (إيميل أو يوزر نيم) 
            // وبما أن القاعدتين اللي فوق بيجبروا الاثنين مع بعض، فكده كده هتبعت الاثنين
            RuleFor(x => x.Password)
                .Must((command, password) => !string.IsNullOrWhiteSpace(command.Email) || !string.IsNullOrWhiteSpace(command.UserName))
                .When(x => !string.IsNullOrWhiteSpace(x.Password))
                .WithMessage("To change Password, you must provide Email or Username for identity verification.");

            // 4. إلزام الباسورد الحالي عند تغيير الباسورد
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.Password))
                .WithMessage("Current password is required to authorize password change.");
        }

        private  void ApplyCustomValidationsRules()
        {
            // 1. Check Email Uniqueness (Using Identity Service)
            RuleFor(x => x.Email)
                .MustAsync(async (command, email, cancellationToken) =>
                {
                    var patient = await unitOfWork.PatientsRepository.GetByIdAsync(command.Id);

                    if (patient == null)
                        return true; // Doctor not found, skip validation

                    var oldEmail = await _identityService.GetUserEmailAsync(patient.ApplicationUserId, cancellationToken);

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
                    var patient = await unitOfWork.PatientsRepository.GetByIdAsync(command.Id);

                    if (patient == null)
                        return true; // Doctor not found, skip validation

                    var oldUserName = await _identityService.GetUserNameAsync(patient.ApplicationUserId, cancellationToken);


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
