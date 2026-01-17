namespace Clinic_System.Application.Features.Prescriptions.Commands.Validators
{
    public class UpdatePrescriptionValidator : AbstractValidator<UpdatePrescriptionCommand>
    {
        public UpdatePrescriptionValidator()
        {
            // 1. Mandatory ID
            RuleFor(x => x.PrescriptionId)
                .NotEmpty().WithMessage("Prescription ID is required.")
                .GreaterThan(0).WithMessage("Invalid Prescription ID.");

            // 2. Conditional Length Checks (Only if provided)
            RuleFor(x => x.MedicationName)
                .MaximumLength(200).WithMessage("Medication name cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.MedicationName));

            RuleFor(x => x.Dosage)
                .MaximumLength(500).WithMessage("Dosage instructions are too long.")
                .When(x => !string.IsNullOrEmpty(x.Dosage));

            RuleFor(x => x.Frequency)
                .MaximumLength(100).WithMessage("Frequency description is too long.")
                .When(x => !string.IsNullOrEmpty(x.Frequency));

            RuleFor(x => x.SpecialInstructions)
                .MaximumLength(1000).WithMessage("Special instructions cannot exceed 1000 characters.");

            // 3. Date Logic (Crucial for medical safety)
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate.Value)
                .WithMessage("End date cannot be earlier than the start date.")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

            // Optional: Ensure StartDate is not in the very distant past
            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithMessage("Start date is too far in the past.")
                .When(x => x.StartDate.HasValue);
        }
    }
}