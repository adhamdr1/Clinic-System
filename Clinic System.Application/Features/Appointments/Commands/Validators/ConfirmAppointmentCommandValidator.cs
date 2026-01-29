namespace Clinic_System.Application.Features.Appointments.Commands.Validators
{
    public class ConfirmAppointmentCommandValidator : AbstractValidator<ConfirmAppointmentCommand>
    {
        private readonly ICurrentUserService _currentUserService; 

        public ConfirmAppointmentCommandValidator(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            // 1. التحقق من الـ IDs
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.")
                .GreaterThan(0).WithMessage("Invalid Appointment ID.");

            RuleFor(x => x.PatientId)
                .MustAsync(async (command, patientId, ct) =>
                {
                    var roles = await _currentUserService.GetCurrentUserRolesAsync();

                    // لو مريض: عدي (حتى لو الـ ID بـ 0)
                    if (roles.Contains("Patient")) return true;

                    // لو أدمن: لازم يكون باعت ID
                    return patientId > 0;
                })
                .WithMessage("Patient ID is required for Admins.");

            // 2. التحقق من المبلغ (Amount)
            RuleFor(x => x.amount)
            .GreaterThan(0)
            .WithMessage("If you provide an amount, it must be greater than zero.")
            .When(x => x.amount.HasValue);

            // 3. التحقق من طريقة الدفع (Enum Validation)
            RuleFor(x => x.method)
                .IsInEnum().WithMessage("Please select a valid payment method (Cash, CreditCard, etc.).");

            // 4. التحقق من الملاحظات (اختياري)
            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
