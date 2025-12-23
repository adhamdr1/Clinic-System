namespace Clinic_System.Application.Features.Patients.Commands.Validators
{
    public class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePatientValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Patient ID is required for update.");


            // تقسيم القواعد لتكون منظمة
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            // Name
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            // Address & Specialization
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            // Phone (Format Only)
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[0-9]{10,15}$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Phone number must contain 10–15 digits (numbers only, optional +)");
        }

        public void ApplyCustomValidationsRules()
        {
            // 3. Check Phone Uniqueness (Using UnitOfWork -> Doctor Repo)
            RuleFor(x => x.Phone)
                .MustAsync(async (command, phone, cancellationToken) =>
                {
                    // Search in Doctors table
                    // تأكد أن لديك ميثود FindAsync أو استخدم AnyAsync لو متاحة
                    var existingDoctor = await _unitOfWork.DoctorsRepository.FindAsync(d => d.Phone == phone);
                    var existingPatient = await _unitOfWork.PatientsRepository.FindAsync(d => d.Phone == phone);
                    
                    bool phoneUsedByOther =
                        existingDoctor.Any()
                        || existingPatient.Any(d => d.Id != command.Id);

                    return !phoneUsedByOther; // Valid if no one else uses it
                })
                .WithMessage("Phone number is already exists")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}
