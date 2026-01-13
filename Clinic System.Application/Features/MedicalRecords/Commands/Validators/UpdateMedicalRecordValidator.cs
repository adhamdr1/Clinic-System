namespace Clinic_System.Application.Features.MedicalRecords.Commands.Validators
{
    public class UpdateMedicalRecordValidator : AbstractValidator<UpdateMedicalRecordCommand>
    {
        public UpdateMedicalRecordValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Medical Record ID is required.");

            // بنقول له: لو بعت داتا، لازم تلتزم بالـ Length.. لو مبعتش (Null or Empty) خلاص فوتها.
            RuleFor(x => x.Diagnosis)
                .MaximumLength(500).WithMessage("Diagnosis cannot exceed 500 characters.")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Diagnosis))
                .WithMessage("Diagnosis must be at least 3 characters if provided.");

            RuleFor(x => x.DescriptionOfTheVisit)
                .MaximumLength(2000).WithMessage("Description is too long.")
                .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.DescriptionOfTheVisit))
                .WithMessage("Description must be at least 10 characters if provided.");

            RuleFor(x => x.AdditionalNotes)
                .MaximumLength(1000).WithMessage("Additional notes cannot exceed 1000 characters.");
        }
    }
}
