namespace Clinic_System.Application.Features.Prescriptions.Commands.Validators
{
    public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionValidator()
        {
            // 1. Medical Record Link
            RuleFor(x => x.MedicalRecordId)
                .NotEmpty().WithMessage("Medical Record ID is required.")
                .GreaterThan(0).WithMessage("Invalid Medical Record ID.");

            // 2. Medication Details
            RuleFor(x => x.MedicationName)
                .NotEmpty().WithMessage("Medication name is required.")
                .MaximumLength(200).WithMessage("Medication name cannot exceed 200 characters.");

            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage instructions are required.")
                .MaximumLength(500).WithMessage("Dosage details are too long.");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency (e.g., 'Twice daily') is required.")
                .MaximumLength(100).WithMessage("Frequency description is too long.");

            // 3. Optional Instructions
            RuleFor(x => x.SpecialInstructions)
                .MaximumLength(1000).WithMessage("Special instructions cannot exceed 1000 characters.");

            // 4. Date Logic
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date cannot be earlier than the start date.");
        }
    }
}