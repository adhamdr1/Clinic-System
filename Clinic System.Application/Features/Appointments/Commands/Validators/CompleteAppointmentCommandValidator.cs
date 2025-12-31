namespace Clinic_System.Application.Features.Appointments.Commands.Validators
{
    public class CompleteAppointmentCommandValidator : AbstractValidator<CompleteAppointmentCommand>
    {
        public CompleteAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0)
                .WithMessage("AppointmentId must be greater than 0.");

            RuleFor(x => x.Diagnosis)
                .NotEmpty()
                .WithMessage("Diagnosis is required.")
                .MaximumLength(1000)
                .WithMessage("Diagnosis is too long.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(2000)
                .WithMessage("Description is too long.");

            RuleForEach(x => x.Medicines).SetValidator(new PrescriptionDtoValidator());
        }
    }

    public class PrescriptionDtoValidator : AbstractValidator<PrescriptionDto>
    {
        public PrescriptionDtoValidator()
        {
            RuleFor(x => x.MedicationName)
                .NotEmpty().WithMessage("MedicationName is required.");

            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage is required.");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("StartDate must be before EndDate.");
        }
    }

}
